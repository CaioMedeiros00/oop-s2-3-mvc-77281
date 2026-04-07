using System.ComponentModel.DataAnnotations;

namespace College.Domain;

public class ExamResult
{
    public int Id { get; set; }
    
    public int ExamId { get; set; }
    public Exam Exam { get; set; } = null!;
    
    public int StudentProfileId { get; set; }
    public StudentProfile StudentProfile { get; set; } = null!;
    
    [Range(0, 1000)]
    public decimal Score { get; set; }
    
    [StringLength(5)]
    public string? Grade { get; set; }
}
