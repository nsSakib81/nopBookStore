using NopBookStore.Models;

namespace NopBookStore.ViewModels
{
    public class BorrowBookViewModel
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public Book Book { get; set; }
        public string UserName { get; set; } // Consider using the actual user ID type from your authentication system
        public DateTime DueDate { get; set; }
    }
}
