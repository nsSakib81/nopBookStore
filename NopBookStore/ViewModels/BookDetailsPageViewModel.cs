using NopBookStore.Models;
using System.ComponentModel.DataAnnotations;

namespace NopBookStore.ViewModels
{
    public class BookDetailsPageViewModel
    {
        [Required]
        public string BookId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Publication Date")]
        [Required]
        public DateTime publicationDate { get; set; }

        [Required]
        public string ISBN { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Language { get; set; }

        [Display(Name = "Cover Photo")]
        [Required]
        public string CoverPhoto { get; set; }

        [Required]
        public string PictureFormat { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public Author Author { get; set; }
        [Display(Name = "Stock Amount")]
        [Required]
        public int StockAmount { get; set; }
    }
}
