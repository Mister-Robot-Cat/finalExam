using System.ComponentModel.DataAnnotations;

namespace FinalExam.Areas.Admin.ViewModels
{
    public class CreateVM
    {
        [Required(ErrorMessage = "Full Name is Require"), MinLength(3, ErrorMessage = "Full Name is too short")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Designation is Require"), MinLength(3, ErrorMessage = "Designation is too short")]

        public string Designation { get; set; }
        public IFormFile Photo { get; set; }
    }
}
