using NopBookStore.Models;
using NopBookStore.ViewModels;

namespace NopBookStore.IServices
{
    public interface IAuthorService
    {
        AuthorViewModel AuthorToAuthorViewModel(Author author);
        IEnumerable<AuthorViewModel> AuthorToAuthorViewModelEnumerable(IEnumerable<Author> author);
        Author AuthorViewModelToAuthor(AuthorViewModel authorViewModel);
        public Task SaveAsync(Author author);
        public Task UpdateAsync(Author author);
        public Task DeleteAsync(Author author);
        public Task<List<Author>> GetAuthorsAsync();
        public Task<List<Author>> GetALlAuthorWithBooksAsync();
        public Task<Author?> FindByIdAsync(string id);
    }
}
