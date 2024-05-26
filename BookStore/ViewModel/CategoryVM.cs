using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class CategoryVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="pleaz enter name")]
        [MaxLength(50, ErrorMessage = "50")]

        public string Name { get; set; } = null!;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime UpdateOn { get; set; } = DateTime.Now;

    }
}
