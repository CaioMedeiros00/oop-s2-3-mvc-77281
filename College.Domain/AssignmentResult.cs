using System.ComponentModel.DataAnnotations;

namespace College.Domain;

public class AssignmentResult
{
    public int Id { get; set; }
    
    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
    
    public int StudentProfileId { get; set; }
    public StudentProfile StudentProfile { get; set; } = null!;
    
    [Range(0, 1000)]
    public decimal Score { get; set; }
    
    public string? Feedback { get; set; }
    
    public DateTime? SubmittedDate { get; set; }
}
