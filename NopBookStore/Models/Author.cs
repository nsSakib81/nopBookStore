using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NopBookStore.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string Description { get; set; }

        public Byte[] AuthorPhoto { get; set; }

        public string PictureFormat { get; set; }

        // Navigation properties 
        public ICollection<Book>? Books { get; set; }
    }
}
