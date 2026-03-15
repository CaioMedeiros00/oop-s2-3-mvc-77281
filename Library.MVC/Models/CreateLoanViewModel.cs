using System.ComponentModel.DataAnnotations;

namespace Library.MVC.Models
{
    public class CreateLoanViewModel
    {
        [Required]
        public int MemberId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime LoanDate { get; set; } = DateTime.Today;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);
    }
}
