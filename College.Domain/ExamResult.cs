using System.ComponentModel.DataAnnotations;

namespace College.Domain
{
    public class ExamResult
    {
        public int Id { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; } = null!;

        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Score { get; set; }

        [StringLength(10)]
        public string Grade { get; set; } = string.Empty;
    }
}
