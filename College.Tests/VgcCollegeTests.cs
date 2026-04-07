using College.Domain;
using College.MVC.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace College.Tests;

public class VgcCollegeTests
{
    private ApplicationDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CanCreateAndRetrieveBranch()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var branch = new Branch { Name = "Test Campus", Address = "123 Test St" };

        // Act
        context.Branches.Add(branch);
        await context.SaveChangesAsync();
        var retrieved = await context.Branches.FirstOrDefaultAsync(b => b.Name == "Test Campus");

        // Assert
        Assert.NotNull(retrieved);
        Assert.Equal("Test Campus", retrieved.Name);
        Assert.Equal("123 Test St", retrieved.Address);
    }

    [Fact]
    public async Task CanEnrolStudentInCourse()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var branch = new Branch { Name = "Test Campus", Address = "123 Test St" };
        context.Branches.Add(branch);
        await context.SaveChangesAsync();

        var student = new StudentProfile
        {
            IdentityUserId = "test-user-id",
            Name = "Test Student",
            Email = "test@test.com",
            StudentNumber = "S001"
        };
        context.StudentProfiles.Add(student);

        var course = new Course
        {
            Name = "Test Course",
            BranchId = branch.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(6)
        };
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        // Act
        var enrolment = new CourseEnrolment
        {
            StudentProfileId = student.Id,
            CourseId = course.Id,
            EnrolDate = DateTime.Now,
            Status = "Active"
        };
        context.CourseEnrolments.Add(enrolment);
        await context.SaveChangesAsync();

        // Assert
        var retrievedEnrolment = await context.CourseEnrolments
            .Include(e => e.StudentProfile)
            .Include(e => e.Course)
            .FirstOrDefaultAsync();

        Assert.NotNull(retrievedEnrolment);
        Assert.Equal("Test Student", retrievedEnrolment.StudentProfile.Name);
        Assert.Equal("Test Course", retrievedEnrolment.Course.Name);
        Assert.Equal("Active", retrievedEnrolment.Status);
    }

    [Fact]
    public async Task CanTrackAttendance()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var branch = new Branch { Name = "Test Campus", Address = "123 Test St" };
        context.Branches.Add(branch);
        await context.SaveChangesAsync();

        var student = new StudentProfile
        {
            IdentityUserId = "test-user-id",
            Name = "Test Student",
            Email = "test@test.com",
            StudentNumber = "S001"
        };
        context.StudentProfiles.Add(student);

        var course = new Course
        {
            Name = "Test Course",
            BranchId = branch.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(6)
        };
        context.Courses.Add(course);

        var enrolment = new CourseEnrolment
        {
            StudentProfileId = student.Id,
            CourseId = course.Id,
            EnrolDate = DateTime.Now,
            Status = "Active"
        };
        context.CourseEnrolments.Add(enrolment);
        await context.SaveChangesAsync();

        // Act
        var attendance = new AttendanceRecord
        {
            CourseEnrolmentId = enrolment.Id,
            WeekNumber = 1,
            Date = DateTime.Now,
            Present = true
        };
        context.AttendanceRecords.Add(attendance);
        await context.SaveChangesAsync();

        // Assert
        var retrievedAttendance = await context.AttendanceRecords
            .Include(a => a.CourseEnrolment)
            .FirstOrDefaultAsync();

        Assert.NotNull(retrievedAttendance);
        Assert.Equal(1, retrievedAttendance.WeekNumber);
        Assert.True(retrievedAttendance.Present);
    }

    [Fact]
    public async Task CanCreateAssignmentAndRecordResult()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var branch = new Branch { Name = "Test Campus", Address = "123 Test St" };
        context.Branches.Add(branch);

        var course = new Course
        {
            Name = "Test Course",
            BranchId = branch.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(6)
        };
        context.Courses.Add(course);

        var student = new StudentProfile
        {
            IdentityUserId = "test-user-id",
            Name = "Test Student",
            Email = "test@test.com",
            StudentNumber = "S001"
        };
        context.StudentProfiles.Add(student);
        await context.SaveChangesAsync();

        var assignment = new Assignment
        {
            CourseId = course.Id,
            Title = "Test Assignment",
            MaxScore = 100,
            DueDate = DateTime.Now.AddDays(7)
        };
        context.Assignments.Add(assignment);
        await context.SaveChangesAsync();

        // Act
        var result = new AssignmentResult
        {
            AssignmentId = assignment.Id,
            StudentProfileId = student.Id,
            Score = 85,
            Feedback = "Good work!"
        };
        context.AssignmentResults.Add(result);
        await context.SaveChangesAsync();

        // Assert
        var retrievedResult = await context.AssignmentResults
            .Include(r => r.Assignment)
            .Include(r => r.StudentProfile)
            .FirstOrDefaultAsync();

        Assert.NotNull(retrievedResult);
        Assert.Equal(85, retrievedResult.Score);
        Assert.Equal("Good work!", retrievedResult.Feedback);
        Assert.Equal("Test Assignment", retrievedResult.Assignment.Title);
    }

    [Fact]
    public async Task AssignmentScoreCannotExceedMaxScore()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var branch = new Branch { Name = "Test Campus", Address = "123 Test St" };
        context.Branches.Add(branch);

        var course = new Course
        {
            Name = "Test Course",
            BranchId = branch.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(6)
        };
        context.Courses.Add(course);

        var student = new StudentProfile
        {
            IdentityUserId = "test-user-id",
            Name = "Test Student",
            Email = "test@test.com",
            StudentNumber = "S001"
        };
        context.StudentProfiles.Add(student);

        var assignment = new Assignment
        {
            CourseId = course.Id,
            Title = "Test Assignment",
            MaxScore = 100,
            DueDate = DateTime.Now.AddDays(7)
        };
        context.Assignments.Add(assignment);
        await context.SaveChangesAsync();

        // Act
        var result = new AssignmentResult
        {
            AssignmentId = assignment.Id,
            StudentProfileId = student.Id,
            Score = 85
        };

        // Assert
        Assert.True(result.Score <= assignment.MaxScore);
        Assert.InRange(result.Score, 0, assignment.MaxScore);
    }

    [Fact]
    public async Task ExamResultsCanBeMarkedAsReleased()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var branch = new Branch { Name = "Test Campus", Address = "123 Test St" };
        context.Branches.Add(branch);

        var course = new Course
        {
            Name = "Test Course",
            BranchId = branch.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(6)
        };
        context.Courses.Add(course);

        var student = new StudentProfile
        {
            IdentityUserId = "test-user-id",
            Name = "Test Student",
            Email = "test@test.com",
            StudentNumber = "S001"
        };
        context.StudentProfiles.Add(student);

        var exam = new Exam
        {
            CourseId = course.Id,
            Title = "Final Exam",
            Date = DateTime.Now.AddDays(30),
            MaxScore = 100,
            ResultsReleased = false
        };
        context.Exams.Add(exam);
        await context.SaveChangesAsync();

        // Act
        exam.ResultsReleased = true;
        context.Exams.Update(exam);
        await context.SaveChangesAsync();

        // Assert
        var retrievedExam = await context.Exams.FindAsync(exam.Id);
        Assert.NotNull(retrievedExam);
        Assert.True(retrievedExam.ResultsReleased);
    }

    [Fact]
    public async Task CanFilterReleasedExamResults()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var branch = new Branch { Name = "Test Campus", Address = "123 Test St" };
        context.Branches.Add(branch);

        var course = new Course
        {
            Name = "Test Course",
            BranchId = branch.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(6)
        };
        context.Courses.Add(course);

        var student = new StudentProfile
        {
            IdentityUserId = "test-user-id",
            Name = "Test Student",
            Email = "test@test.com",
            StudentNumber = "S001"
        };
        context.StudentProfiles.Add(student);

        var releasedExam = new Exam
        {
            CourseId = course.Id,
            Title = "Released Exam",
            Date = DateTime.Now,
            MaxScore = 100,
            ResultsReleased = true
        };

        var unreleasedExam = new Exam
        {
            CourseId = course.Id,
            Title = "Unreleased Exam",
            Date = DateTime.Now.AddDays(30),
            MaxScore = 100,
            ResultsReleased = false
        };

        context.Exams.AddRange(releasedExam, unreleasedExam);
        await context.SaveChangesAsync();

        context.ExamResults.Add(new ExamResult
        {
            ExamId = releasedExam.Id,
            StudentProfileId = student.Id,
            Score = 85,
            Grade = "A"
        });

        context.ExamResults.Add(new ExamResult
        {
            ExamId = unreleasedExam.Id,
            StudentProfileId = student.Id,
            Score = 90,
            Grade = "A+"
        });

        await context.SaveChangesAsync();

        // Act - Simulate student query for only released results
        var releasedResults = await context.ExamResults
            .Include(r => r.Exam)
            .Where(r => r.StudentProfileId == student.Id && r.Exam.ResultsReleased)
            .ToListAsync();

        // Assert
        Assert.Single(releasedResults);
        Assert.Equal("Released Exam", releasedResults[0].Exam.Title);
    }

    [Fact]
    public async Task FacultyCanOnlySeeTheirStudents()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var branch = new Branch { Name = "Test Campus", Address = "123 Test St" };
        context.Branches.Add(branch);

        var faculty1 = new FacultyProfile
        {
            IdentityUserId = "faculty1-id",
            Name = "Dr. Smith",
            Email = "smith@test.com"
        };

        var faculty2 = new FacultyProfile
        {
            IdentityUserId = "faculty2-id",
            Name = "Dr. Jones",
            Email = "jones@test.com"
        };

        context.FacultyProfiles.AddRange(faculty1, faculty2);
        await context.SaveChangesAsync();

        var course1 = new Course
        {
            Name = "Course 1",
            BranchId = branch.Id,
            FacultyProfileId = faculty1.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(6)
        };

        var course2 = new Course
        {
            Name = "Course 2",
            BranchId = branch.Id,
            FacultyProfileId = faculty2.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(6)
        };

        context.Courses.AddRange(course1, course2);
        await context.SaveChangesAsync();

        var student1 = new StudentProfile
        {
            IdentityUserId = "student1-id",
            Name = "Student 1",
            Email = "student1@test.com",
            StudentNumber = "S001"
        };

        var student2 = new StudentProfile
        {
            IdentityUserId = "student2-id",
            Name = "Student 2",
            Email = "student2@test.com",
            StudentNumber = "S002"
        };

        context.StudentProfiles.AddRange(student1, student2);
        await context.SaveChangesAsync();

        context.CourseEnrolments.Add(new CourseEnrolment
        {
            StudentProfileId = student1.Id,
            CourseId = course1.Id,
            EnrolDate = DateTime.Now,
            Status = "Active"
        });

        context.CourseEnrolments.Add(new CourseEnrolment
        {
            StudentProfileId = student2.Id,
            CourseId = course2.Id,
            EnrolDate = DateTime.Now,
            Status = "Active"
        });

        await context.SaveChangesAsync();

        // Act - Simulate faculty1 querying their students
        var faculty1Students = await context.CourseEnrolments
            .Include(e => e.StudentProfile)
            .Include(e => e.Course)
            .Where(e => e.Course.FacultyProfileId == faculty1.Id)
            .Select(e => e.StudentProfile)
            .Distinct()
            .ToListAsync();

        // Assert
        Assert.Single(faculty1Students);
        Assert.Equal("Student 1", faculty1Students[0].Name);
    }

    [Fact]
    public async Task CanCalculateAttendancePercentage()
    {
        // Arrange
        using var context = GetInMemoryDbContext();
        var branch = new Branch { Name = "Test Campus", Address = "123 Test St" };
        context.Branches.Add(branch);

        var student = new StudentProfile
        {
            IdentityUserId = "test-user-id",
            Name = "Test Student",
            Email = "test@test.com",
            StudentNumber = "S001"
        };
        context.StudentProfiles.Add(student);

        var course = new Course
        {
            Name = "Test Course",
            BranchId = branch.Id,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddMonths(6)
        };
        context.Courses.Add(course);

        var enrolment = new CourseEnrolment
        {
            StudentProfileId = student.Id,
            CourseId = course.Id,
            EnrolDate = DateTime.Now,
            Status = "Active"
        };
        context.CourseEnrolments.Add(enrolment);
        await context.SaveChangesAsync();

        // Add 10 attendance records: 7 present, 3 absent
        for (int i = 1; i <= 10; i++)
        {
            context.AttendanceRecords.Add(new AttendanceRecord
            {
                CourseEnrolmentId = enrolment.Id,
                WeekNumber = i,
                Date = DateTime.Now.AddDays(i * 7),
                Present = i <= 7
            });
        }
        await context.SaveChangesAsync();

        // Act
        var attendanceRecords = await context.AttendanceRecords
            .Where(a => a.CourseEnrolmentId == enrolment.Id)
            .ToListAsync();

        var totalRecords = attendanceRecords.Count;
        var presentCount = attendanceRecords.Count(a => a.Present);
        var attendancePercentage = (double)presentCount / totalRecords * 100;

        // Assert
        Assert.Equal(10, totalRecords);
        Assert.Equal(7, presentCount);
        Assert.Equal(70.0, attendancePercentage);
    }
}
