using System.ComponentModel.DataAnnotations;

namespace FinalExam.Models
{
    public class Chef : BaseEntity
    {
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string ImageUrl { get; set; }
    }
}
