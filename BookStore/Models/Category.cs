using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Models
{
    [Index(nameof(Name),IsUnique =true)]
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdateOn { get; set; } = DateTime.Now;
        public List<BookCategory> Books { get; set; } = new List<BookCategory>();

    }
}
