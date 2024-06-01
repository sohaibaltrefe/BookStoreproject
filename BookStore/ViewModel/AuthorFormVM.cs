using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModel
{
    public class AuthorFormVM
    {
      public int Id { get; set; }
        [MaxLength(35,ErrorMessage = "the name field can't exceed 35 characters ")]

        public string Name { get; set; }
    }
}
