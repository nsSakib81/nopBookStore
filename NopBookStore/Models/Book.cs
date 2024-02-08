using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NopBookStore.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BookId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime publicationDate { get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }

        public Byte[] CoverPhoto { get; set; }

        public string PictureFormat { get; set; }

        // navigation properties 
        [ForeignKey("Author")]
        public string AuthorId { get; set; }
        public Author Author { get; set; }

        public int StockAmount { get; set; }
    }
}
