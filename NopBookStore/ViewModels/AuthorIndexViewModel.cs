using System.ComponentModel.DataAnnotations;

namespace NopBookStore.ViewModels
{
    public class AuthorIndexViewModel
    {
        [Required]
        public string AuthorId { get; set; }

        [Display(Name = "Author Name")]
        [Required]
        public string AuthorName { get; set; }

        [Display(Name = "Author Eamil")]
        [Required]
        public string AuthorEmail { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Author Photo")]
        [Required]
        public string AuthorPhoto { get; set; }

        [Required]
        public string PictureFormat { get; set; }
    }
}
