using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NopBookStore.Data;
using NopBookStore.ViewModels;
using Microsoft.EntityFrameworkCore;
using NopBookStore.Models;
using System.Security.Claims;
using NopBookStore.IServices;
using NopBookStore.Middleware;
using AutoMapper;


namespace NopBookStore.Controllers
{
    public class BookController : Controller
    {
        private readonly ModernBookShopDbContext modernBookShopDbContext;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IUserService _userService;
        private readonly ICurrentUser _currentUser;
        private IMapper _mapper;

        public BookController(ModernBookShopDbContext context, IRolePermissionService rolePermissionService, IUserService userService, ICurrentUser currentUser, IMapper mapper)
        {
            modernBookShopDbContext = context;
            _rolePermissionService = rolePermissionService;
            _userService = userService;
            _currentUser = currentUser;
            _mapper = mapper;
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


        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Title = "Updation of Book Details";
            if (id == null || modernBookShopDbContext.Books == null)
            {
                return NotFound();
            }

            var book = await modernBookShopDbContext.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            var authors = modernBookShopDbContext.Authors.ToList();
            ViewBag.Authors = authors;
            // from Book model object to BookEditViewModel object
            var bookEditViewModel = new BookEditViewModel()
            {
                AuthorId = book.AuthorId,
                BookId = book.BookId,
                Genre = book.Genre,
                ISBN = book.ISBN,
                Language = book.Language,
                Title = book.Title,
                StockAmount = book.StockAmount,
                Description = book.Description,
                publicationDate = book.publicationDate

            };

            var stream = new MemoryStream(book.CoverPhoto);
            IFormFile formFile = new FormFile(stream, 0, book.CoverPhoto.Length, "name", "fileName");
            bookEditViewModel.CoverPhoto = formFile;

            return View(bookEditViewModel);
        }


        // POST: Books/Edit/idAutoMapper.AutoMapperMappingException: 'Error mapping types.'

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, BookEditViewModel bookEditViewModel)
        {
            if (id != bookEditViewModel.BookId) // ensuring security 
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var book = _mapper.Map<Book>(bookEditViewModel);
                    if(bookEditViewModel.CoverPhoto!= null && !string.IsNullOrEmpty(bookEditViewModel.CoverPhoto.ContentType))
                    {
                        book.PictureFormat= bookEditViewModel.CoverPhoto.ContentType;
                    }
                    else
                    {
                        book.PictureFormat = "defaultFormat";
                    }
                    //{
                    //  Title = bookEditViewModel.Title,
                    //  Genre = bookEditViewModel.Genre,
                    //  Description = bookEditViewModel.Description,
                    //  ISBN = bookEditViewModel.ISBN,
                    //  Language = bookEditViewModel.Language,
                    //  publicationDate = bookEditViewModel.publicationDate,
                    //  StockAmount = bookEditViewModel.StockAmount,
                    //  AuthorId = bookEditViewModel.AuthorId,
                    //  PictureFormat = bookEditViewModel.CoverPhoto.ContentType
                    //};

                    // converting the coverPhoto from FormFile to byte array
                    using (var memoryStream = new MemoryStream())
                    {
                        await bookEditViewModel.CoverPhoto.CopyToAsync(memoryStream);
                        book.CoverPhoto = memoryStream.ToArray();
                    }

                    // getting the author of the book 
                    var authorOfTheBook = await modernBookShopDbContext.Authors.FindAsync(bookEditViewModel.AuthorId);
                    if (authorOfTheBook == null)
                    {
                        return Problem("Author of the book not found in the 'BookShopDbContext.Author' entity");
                    }
                    book.Author = authorOfTheBook;

                    // adding to the context
                    modernBookShopDbContext.Update(book);
                    await modernBookShopDbContext.SaveChangesAsync(); // adding to the database
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(bookEditViewModel.BookId))
                    {
                        if (!BookExists(bookEditViewModel.BookId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw new Exception("Error from the BookController Post Edit method!");
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            var authors = await modernBookShopDbContext.Authors.ToListAsync();
            ViewBag.Authors = authors;
            return View(bookEditViewModel);
        }
        private bool BookExists(string id)
        {
            return (modernBookShopDbContext.Books?.Any(b => b.BookId == id)).GetValueOrDefault();
        }
    }
}