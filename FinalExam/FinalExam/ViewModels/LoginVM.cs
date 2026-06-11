using System.ComponentModel.DataAnnotations;

namespace FinalExam.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserNameOrEmail { get; set; }
        [DataType(DataType.Password), Required]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
