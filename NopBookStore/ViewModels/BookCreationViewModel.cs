using System.ComponentModel.DataAnnotations;

namespace NopBookStore.ViewModels
{
    public class BookCreationViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Publiaction Date")]
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
        public IFormFile CoverPhoto { get; set; }

        [Display(Name = "Author")]
        [Required]
        public string AuthorId { get; set; }
        [Display(Name = "Stock Amount")]
        [Required]
        public int StockAmount { get; set; }
    }
}
