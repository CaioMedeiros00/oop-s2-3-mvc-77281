using System.ComponentModel.DataAnnotations;

namespace College.Domain
{
    public class AssignmentResult
    {
        public int Id { get; set; }

        public int AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;

        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; } = null!;

        [Range(0, int.MaxValue)]
        public int Score { get; set; }

        [StringLength(500)]
        public string Feedback { get; set; } = string.Empty;
    }
}
