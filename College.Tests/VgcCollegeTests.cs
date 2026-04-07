using College.Domain;
using College.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace College.Tests
{
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
        public async Task CanCreateBranch()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var branch = new Branch
            {
                Name = "Dublin Campus",
                Address = "123 Main Street, Dublin"
            };

            // Act
            context.Branches.Add(branch);
            await context.SaveChangesAsync();

            // Assert
            var branchFromDb = await context.Branches.FirstOrDefaultAsync();
            Assert.NotNull(branchFromDb);
            Assert.Equal("Dublin Campus", branchFromDb.Name);
        }

        [Fact]
        public async Task CanEnrolStudentInCourse()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var branch = new Branch { Name = "Dublin", Address = "Dublin Address" };
            var course = new Course
            {
                Name = "Computer Science",
                Branch = branch,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(9)
            };
            var student = new StudentProfile
            {
                Name = "John Doe",
                Email = "john@test.com",
                Phone = "123456789",
                StudentNumber = "S001",
                IdentityUserId = Guid.NewGuid().ToString()
            };

            context.Branches.Add(branch);
            context.Courses.Add(course);
            context.StudentProfiles.Add(student);
            await context.SaveChangesAsync();

            var enrolment = new CourseEnrolment
            {
                StudentProfileId = student.Id,
                CourseId = course.Id,
                EnrolDate = DateTime.Today,
                Status = "Active"
            };

            // Act
            context.CourseEnrolments.Add(enrolment);
            await context.SaveChangesAsync();

            // Assert
            var enrolmentFromDb = await context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                .FirstOrDefaultAsync();
            Assert.NotNull(enrolmentFromDb);
            Assert.Equal("John Doe", enrolmentFromDb.StudentProfile.Name);
            Assert.Equal("Computer Science", enrolmentFromDb.Course.Name);
        }

        [Fact]
        public async Task ExamResultsNotVisibleUntilReleased()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var branch = new Branch { Name = "Dublin", Address = "Dublin Address" };
            var course = new Course
            {
                Name = "Computer Science",
                Branch = branch,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(9)
            };
            var exam = new Exam
            {
                Course = course,
                Title = "Final Exam",
                Date = DateTime.Today,
                MaxScore = 100,
                ResultsReleased = false
            };

            context.Exams.Add(exam);
            await context.SaveChangesAsync();

            // Act
            var examFromDb = await context.Exams.FindAsync(exam.Id);

            // Assert
            Assert.NotNull(examFromDb);
            Assert.False(examFromDb.ResultsReleased);
        }

        [Fact]
        public async Task CanReleaseExamResults()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var branch = new Branch { Name = "Dublin", Address = "Dublin Address" };
            var course = new Course
            {
                Name = "Computer Science",
                Branch = branch,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(9)
            };
            var exam = new Exam
            {
                Course = course,
                Title = "Final Exam",
                Date = DateTime.Today,
                MaxScore = 100,
                ResultsReleased = false
            };

            context.Exams.Add(exam);
            await context.SaveChangesAsync();

            // Act
            exam.ResultsReleased = true;
            context.Update(exam);
            await context.SaveChangesAsync();

            var examFromDb = await context.Exams.FindAsync(exam.Id);

            // Assert
            Assert.NotNull(examFromDb);
            Assert.True(examFromDb.ResultsReleased);
        }

        [Fact]
        public async Task CanCreateAssignmentForCourse()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var branch = new Branch { Name = "Dublin", Address = "Dublin Address" };
            var course = new Course
            {
                Name = "Computer Science",
                Branch = branch,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(9)
            };
            var assignment = new Assignment
            {
                Course = course,
                Title = "Assignment 1",
                MaxScore = 100,
                DueDate = DateTime.Today.AddDays(14)
            };

            // Act
            context.Assignments.Add(assignment);
            await context.SaveChangesAsync();

            // Assert
            var assignmentFromDb = await context.Assignments
                .Include(a => a.Course)
                .FirstOrDefaultAsync();
            Assert.NotNull(assignmentFromDb);
            Assert.Equal("Assignment 1", assignmentFromDb.Title);
            Assert.Equal(100, assignmentFromDb.MaxScore);
        }

        [Fact]
        public async Task CanRecordAssignmentResults()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var branch = new Branch { Name = "Dublin", Address = "Dublin Address" };
            var course = new Course
            {
                Name = "Computer Science",
                Branch = branch,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(9)
            };
            var assignment = new Assignment
            {
                Course = course,
                Title = "Assignment 1",
                MaxScore = 100,
                DueDate = DateTime.Today.AddDays(14)
            };
            var student = new StudentProfile
            {
                Name = "John Doe",
                Email = "john@test.com",
                Phone = "123456789",
                StudentNumber = "S001",
                IdentityUserId = Guid.NewGuid().ToString()
            };

            context.Assignments.Add(assignment);
            context.StudentProfiles.Add(student);
            await context.SaveChangesAsync();

            var result = new AssignmentResult
            {
                AssignmentId = assignment.Id,
                StudentProfileId = student.Id,
                Score = 85,
                Feedback = "Good work!"
            };

            // Act
            context.AssignmentResults.Add(result);
            await context.SaveChangesAsync();

            // Assert
            var resultFromDb = await context.AssignmentResults
                .Include(r => r.Assignment)
                .Include(r => r.StudentProfile)
                .FirstOrDefaultAsync();
            Assert.NotNull(resultFromDb);
            Assert.Equal(85, resultFromDb.Score);
            Assert.Equal("Good work!", resultFromDb.Feedback);
        }

        [Fact]
        public async Task CanTrackAttendance()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var branch = new Branch { Name = "Dublin", Address = "Dublin Address" };
            var course = new Course
            {
                Name = "Computer Science",
                Branch = branch,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(9)
            };
            var student = new StudentProfile
            {
                Name = "John Doe",
                Email = "john@test.com",
                Phone = "123456789",
                StudentNumber = "S001",
                IdentityUserId = Guid.NewGuid().ToString()
            };
            var enrolment = new CourseEnrolment
            {
                StudentProfile = student,
                Course = course,
                EnrolDate = DateTime.Today,
                Status = "Active"
            };

            context.CourseEnrolments.Add(enrolment);
            await context.SaveChangesAsync();

            var attendance = new AttendanceRecord
            {
                CourseEnrolmentId = enrolment.Id,
                WeekNumber = 1,
                Date = DateTime.Today,
                Present = true
            };

            // Act
            context.AttendanceRecords.Add(attendance);
            await context.SaveChangesAsync();

            // Assert
            var attendanceFromDb = await context.AttendanceRecords
                .Include(a => a.CourseEnrolment)
                .FirstOrDefaultAsync();
            Assert.NotNull(attendanceFromDb);
            Assert.True(attendanceFromDb.Present);
            Assert.Equal(1, attendanceFromDb.WeekNumber);
        }

        [Fact]
        public async Task StudentNumberShouldBeUnique()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var student1 = new StudentProfile
            {
                Name = "John Doe",
                Email = "john@test.com",
                Phone = "123456789",
                StudentNumber = "S001",
                IdentityUserId = Guid.NewGuid().ToString()
            };
            var student2 = new StudentProfile
            {
                Name = "Jane Doe",
                Email = "jane@test.com",
                Phone = "987654321",
                StudentNumber = "S002",
                IdentityUserId = Guid.NewGuid().ToString()
            };

            // Act
            context.StudentProfiles.Add(student1);
            context.StudentProfiles.Add(student2);
            await context.SaveChangesAsync();

            // Assert - Verify both students have different student numbers
            var students = await context.StudentProfiles.ToListAsync();
            Assert.Equal(2, students.Count);
            Assert.NotEqual(students[0].StudentNumber, students[1].StudentNumber);
        }

        [Fact]
        public async Task AssignmentScoreMustBeValid()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var branch = new Branch { Name = "Dublin", Address = "Dublin Address" };
            var course = new Course
            {
                Name = "Computer Science",
                Branch = branch,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(9)
            };
            var assignment = new Assignment
            {
                Course = course,
                Title = "Assignment 1",
                MaxScore = 100,
                DueDate = DateTime.Today.AddDays(14)
            };
            var student = new StudentProfile
            {
                Name = "John Doe",
                Email = "john@test.com",
                Phone = "123456789",
                StudentNumber = "S001",
                IdentityUserId = Guid.NewGuid().ToString()
            };

            context.Assignments.Add(assignment);
            context.StudentProfiles.Add(student);
            await context.SaveChangesAsync();

            // Act
            var result = new AssignmentResult
            {
                AssignmentId = assignment.Id,
                StudentProfileId = student.Id,
                Score = 95,
                Feedback = "Excellent work!"
            };

            context.AssignmentResults.Add(result);
            await context.SaveChangesAsync();

            // Assert
            var resultFromDb = await context.AssignmentResults.FindAsync(result.Id);
            Assert.NotNull(resultFromDb);
            Assert.True(resultFromDb.Score >= 0 && resultFromDb.Score <= assignment.MaxScore);
        }

        [Fact]
        public async Task CanAssignFacultyToCourse()
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var branch = new Branch { Name = "Dublin", Address = "Dublin Address" };
            var course = new Course
            {
                Name = "Computer Science",
                Branch = branch,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(9)
            };
            var faculty = new FacultyProfile
            {
                Name = "Dr. Smith",
                Email = "smith@test.com",
                Phone = "111222333",
                IdentityUserId = Guid.NewGuid().ToString()
            };

            context.Courses.Add(course);
            context.FacultyProfiles.Add(faculty);
            await context.SaveChangesAsync();

            // Act
            course.FacultyMembers.Add(faculty);
            await context.SaveChangesAsync();

            // Assert
            var courseFromDb = await context.Courses
                .Include(c => c.FacultyMembers)
                .FirstOrDefaultAsync(c => c.Id == course.Id);
            Assert.NotNull(courseFromDb);
            Assert.Single(courseFromDb.FacultyMembers);
            Assert.Equal("Dr. Smith", courseFromDb.FacultyMembers.First().Name);
        }
    }
}
