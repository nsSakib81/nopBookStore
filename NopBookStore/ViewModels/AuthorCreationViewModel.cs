using System.ComponentModel.DataAnnotations;

namespace NopBookStore.ViewModels
{
    public class AuthorCreationViewModel
    {
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
        public IFormFile AuthorPhoto { get; set; }
    }
}
