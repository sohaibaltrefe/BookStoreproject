using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    public class Author
    {
        public int Id { get; set; }
        [MaxLength(35)]

        public string? Name { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdateOn { get; set; } = DateTime.Now;
    }
}
