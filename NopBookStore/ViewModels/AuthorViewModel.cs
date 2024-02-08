using System.ComponentModel.DataAnnotations;

namespace NopBookStore.ViewModels
{
    public class AuthorViewModel
    {
        [Required]
        public string AuthorId { get; set; }

        [Display(Name = "Author Name")]
        [Required]
        public string AuthorName { get; set; }

        [Display(Name = "Author Email")]
        [Required]
        public string AuthorEmail { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Author Photo")]
        [Required]
        public string AuthorPhoto { get; set; }

        [Required]
        public IFormFile AuthorPhotoFile { get; set; }

        [Required]
        public string PictureFormat { get; set; }

        public ICollection<BookViewModel>? Books { get; set; }
    }
}
