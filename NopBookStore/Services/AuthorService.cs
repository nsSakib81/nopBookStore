using NopBookStore.Data;
using NopBookStore.IServices;
using NopBookStore.Models;
using NopBookStore.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace NopBookStore.Services
{
    public class AuthorService: IAuthorService
    {
        
        private readonly ModernBookShopDbContext modernBookShopDbContext;
        private readonly IHttpContextAccessor contextAccessor;
        private  ClaimsPrincipal? user;

        public AuthorService(ModernBookShopDbContext context,
            IHttpContextAccessor contextAccessor)
        {
            modernBookShopDbContext = context;
            this.contextAccessor = contextAccessor;

        }

        public void setUser()
        {
            this.user = contextAccessor.HttpContext.User;
        
        }
        public AuthorViewModel AuthorToAuthorViewModel(Author author)
        {
            var authorViewModel = new AuthorViewModel()
            {
                AuthorEmail = author?.AuthorEmail ?? string.Empty,
                AuthorName = author?.AuthorName ?? string.Empty,
                AuthorId = author?.AuthorId ?? string.Empty,
                Description = author?.Description ?? string.Empty,
                PictureFormat = author?.PictureFormat ?? string.Empty,
                AuthorPhoto = author?.AuthorPhoto != null ? Convert.ToBase64String(author.AuthorPhoto) : "",
                Books = new List<BookViewModel>()
            };

            if (author?.AuthorPhoto != null)
            {
                var stream = new MemoryStream(author.AuthorPhoto);
                IFormFile file = new FormFile(stream, 0, author.AuthorPhoto.Length, "name", "filename");
                authorViewModel.AuthorPhotoFile = file;
            }

            if (author?.Books != null)
            {
                foreach (var book in author.Books)
                {
                    var bookViewModel = new BookViewModel()
                    {
                        BookId = book.BookId,
                        AuthorId = book.AuthorId,
                        Genre = book.Genre,
                        Description = book.Description,
                        ISBN = book.ISBN,
                        Language = book.Language,
                        Title = book.Title,
                        publicationDate = book.publicationDate,
                        CoverPhoto = Convert.ToBase64String(book.CoverPhoto)
                    };
                    authorViewModel.Books.Add(bookViewModel);
                }
            }
            return authorViewModel;
        }

        public IEnumerable<AuthorViewModel> AuthorToAuthorViewModelEnumerable(IEnumerable<Author> author)
        {
            var authorViewModel = new List<AuthorViewModel>();

            if (author != null)
            {
                foreach (var eachAuthor in author)
                {
                    authorViewModel.Add(AuthorToAuthorViewModel(eachAuthor));
                }
            }

            return authorViewModel;
        }

        public Author AuthorViewModelToAuthor(AuthorViewModel authorViewModel)
        {
            var author = new Author()
            {
                AuthorEmail = authorViewModel?.AuthorEmail ?? string.Empty,
                AuthorName = authorViewModel?.AuthorName ?? string.Empty,
                AuthorId = authorViewModel?.AuthorId ?? string.Empty,
                Description = authorViewModel?.Description ?? string.Empty,
                PictureFormat = authorViewModel?.AuthorPhotoFile.ContentType ?? string.Empty,
            };
            var memoryStream = new MemoryStream();
            authorViewModel?.AuthorPhotoFile.CopyTo(memoryStream);
            author.AuthorPhoto = memoryStream.ToArray();

            return author;
        }


        public async Task SaveAsync(Author author)
        {
            modernBookShopDbContext.Add(author);
            await modernBookShopDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Author author)
        {
            modernBookShopDbContext.Update(author);
            await modernBookShopDbContext.SaveChangesAsync();
        }
        public async Task DeleteAsync(Author author)
        {
            modernBookShopDbContext.Authors.Remove(author);
            await modernBookShopDbContext.SaveChangesAsync();
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            var authors = await modernBookShopDbContext.Authors.ToListAsync();
            return authors;
        }

        public async Task<List<Author>> GetAllAuthorWithBooksAsync()
        {
            var authors = await modernBookShopDbContext.Authors
                .Include(a => a.Books)
                .ToListAsync();
            return authors;
        }

        public async Task<Author?> FindByIdAsync(string id)
        {
            var author = await modernBookShopDbContext.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(m => m.AuthorId == id);
            return author;
        }

        public Task<List<Author>> GetALlAuthorWithBooksAsync()
        {
            throw new NotImplementedException();
        }
    }
}
