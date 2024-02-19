using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NopBookStore.Data;
using NopBookStore.ViewModels;
using Microsoft.EntityFrameworkCore;
using NopBookStore.Models;
using System.Security.Claims;
using NopBookStore.IServices;
using NopBookStore.Middleware;


namespace NopBookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly ModernBookShopDbContext modernBookShopDbContext;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;

        public BookController(ModernBookShopDbContext context, IRolePermissionService rolePermissionService, IUserService userService, ICurrentUser currentUser)
        {
            modernBookShopDbContext = context;
            _rolePermissionService = rolePermissionService;
            _userService = userService;
            _currentUser = currentUser;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {

            ViewBag.Title = "List of the books";

            var booksWithAuthors = await modernBookShopDbContext.Books.Include(b => b.Author).ToListAsync();
            if (booksWithAuthors == null)
            {
                return NotFound();
            }

            var booksWithAuthorsViewModel = new List<BookIndexPageViewModel>();
            foreach (var book in booksWithAuthors)
            {
                var bookWithAuthorViewModel = new BookIndexPageViewModel()
                {
                    BookId = book.BookId,
                    Author = book.Author,
                    Title = book.Title,
                    PictureFormat = book.PictureFormat,
                    Description = book.Description,
                    Genre = book.Genre,
                    ISBN = book.ISBN,
                    publicationDate = book.publicationDate,
                    Language = book.Language,
                    CoverPhoto = Convert.ToBase64String(book.CoverPhoto),
                    AuthorId = book.AuthorId,
                    StockAmount = book.StockAmount

                };
                booksWithAuthorsViewModel.Add(bookWithAuthorViewModel);
            }

            return View(booksWithAuthorsViewModel);
        }
        public async Task<IActionResult> Create()
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value; // Assuming email is stored as a claim
            var userId = _currentUser.GetUserId();
            if (userId != null && !await HasPermission(userId.Value, "Book.Add"))
            {
                return RedirectToAction("Index", "Book"); // Or redirect to an access denied page
            }
            ViewBag.Title = "Add the Book Details";

            var authors = await modernBookShopDbContext.Authors.ToListAsync();
            if (authors == null)
            {
                authors = new List<Author>(); // or handle the scenario accordingly
            }
            ViewBag.Authors = authors;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookCreationViewModel bookCreationViewModel)
        {
            // Retrieve the user's role
            var userId = _currentUser.GetUserId();
            if (userId != null && !await HasPermission(Convert.ToInt32(userId), "Book.Add"))
            {
                return RedirectToAction("Index", "Book"); // Or redirect to an access denied page
            }

            // The rest of your Create action logic...

            if (bookCreationViewModel == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // convert viewModel object to book model object
                var book = new Book()
                {
                    Title = bookCreationViewModel.Title,
                    Genre = bookCreationViewModel.Genre,
                    Description = bookCreationViewModel.Description,
                    ISBN = bookCreationViewModel.ISBN,
                    Language = bookCreationViewModel.Language,
                    publicationDate = bookCreationViewModel.publicationDate,
                    StockAmount = bookCreationViewModel.StockAmount,
                    AuthorId = bookCreationViewModel.AuthorId,
                    PictureFormat = bookCreationViewModel.CoverPhoto.ContentType
                };

                // converting the coverPhoto from FormFile to byte array
                var memoryStream = new MemoryStream();
                bookCreationViewModel.CoverPhoto.CopyTo(memoryStream);
                book.CoverPhoto = memoryStream.ToArray();

                // getting the author of the book 
                var authorOfTheBook = await modernBookShopDbContext.Authors.FindAsync(bookCreationViewModel.AuthorId);
                if (authorOfTheBook == null)
                {
                    return Problem("Author of the book not found in the 'BookShopDbContext.Author' entity");
                }
                book.Author = authorOfTheBook;

                // adding to the context
                modernBookShopDbContext.Add(book);
                await modernBookShopDbContext.SaveChangesAsync(); // adding to the database
                return RedirectToAction(nameof(Index));
            }

            // Handle the case where ModelState is not valid
            var authors = await modernBookShopDbContext.Authors.ToListAsync();
            ViewBag.Authors = authors;
            return View(bookCreationViewModel);
        }

        private async Task<bool> HasPermission(int userId, string permissionName)
        {
            //if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(permissionName))
            //{
            //    return false;
            //}
            // get the user
            //var user = await _userService.GetUserByEmail(userEmail);
            //if (user == null)
            //{
            //    return false; // User not found
            //}

            var rolePermissions = await _rolePermissionService.GetRolePermissionsAsync(userId);
            return rolePermissions != null && rolePermissions.Contains(permissionName);
        }

    }
}