using System.ComponentModel.DataAnnotations;

namespace College.Domain
{
    public class Assignment
    {
        public int Id { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public int MaxScore { get; set; }
        public DateTime DueDate { get; set; }

        public ICollection<AssignmentResult> Results { get; set; } = new List<AssignmentResult>();
    }
}
