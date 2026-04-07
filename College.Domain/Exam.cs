using System.ComponentModel.DataAnnotations;

namespace College.Domain;

public class Exam
{
    public int Id { get; set; }
    
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    public DateTime Date { get; set; }
    
    [Range(0, 1000)]
    public decimal MaxScore { get; set; }
    
    public bool ResultsReleased { get; set; } = false;
    
    public ICollection<ExamResult> Results { get; set; } = new List<ExamResult>();
}
