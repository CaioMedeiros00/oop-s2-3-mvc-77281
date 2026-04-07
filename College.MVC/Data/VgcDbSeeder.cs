using College.Domain;
using Microsoft.AspNetCore.Identity;

namespace College.MVC.Data;

public static class VgcDbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Create roles if they don't exist
        string[] roles = { "Admin", "Faculty", "Student" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed Admin User
        var adminEmail = "admin@vgc.ie";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(admin, "Admin123!");
            await userManager.AddToRoleAsync(admin, "Admin");
        }

        // Seed Faculty User
        var facultyEmail = "faculty@vgc.ie";
        IdentityUser? facultyUser = await userManager.FindByEmailAsync(facultyEmail);
        if (facultyUser == null)
        {
            facultyUser = new IdentityUser
            {
                UserName = facultyEmail,
                Email = facultyEmail,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(facultyUser, "Faculty123!");
            await userManager.AddToRoleAsync(facultyUser, "Faculty");
        }

        // Seed Student Users
        var student1Email = "student1@vgc.ie";
        IdentityUser? student1User = await userManager.FindByEmailAsync(student1Email);
        if (student1User == null)
        {
            student1User = new IdentityUser
            {
                UserName = student1Email,
                Email = student1Email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(student1User, "Student123!");
            await userManager.AddToRoleAsync(student1User, "Student");
        }

        var student2Email = "student2@vgc.ie";
        IdentityUser? student2User = await userManager.FindByEmailAsync(student2Email);
        if (student2User == null)
        {
            student2User = new IdentityUser
            {
                UserName = student2Email,
                Email = student2Email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(student2User, "Student123!");
            await userManager.AddToRoleAsync(student2User, "Student");
        }

        // Seed Branches
        if (!context.Branches.Any())
        {
            var branches = new[]
            {
                new Branch { Name = "Dublin Campus", Address = "123 O'Connell Street, Dublin 1" },
                new Branch { Name = "Cork Campus", Address = "45 Patrick Street, Cork" },
                new Branch { Name = "Galway Campus", Address = "78 Shop Street, Galway" }
            };
            context.Branches.AddRange(branches);
            await context.SaveChangesAsync();
        }

        // Seed Faculty Profile
        facultyUser = await userManager.FindByEmailAsync(facultyEmail);
        if (!context.FacultyProfiles.Any() && facultyUser != null)
        {
            var facultyProfile = new FacultyProfile
            {
                IdentityUserId = facultyUser.Id,
                Name = "Dr. John Smith",
                Email = facultyEmail,
                Phone = "01-234-5678"
            };
            context.FacultyProfiles.Add(facultyProfile);
            await context.SaveChangesAsync();
        }

        // Seed Student Profiles
        if (!context.StudentProfiles.Any())
        {
            student1User = await userManager.FindByEmailAsync(student1Email);
            student2User = await userManager.FindByEmailAsync(student2Email);

            if (student1User != null && student2User != null)
            {
                var students = new[]
                {
                    new StudentProfile
                    {
                        IdentityUserId = student1User.Id,
                        Name = "Alice Johnson",
                        Email = student1Email,
                        Phone = "087-111-2222",
                        Address = "10 Main Street, Dublin",
                        DateOfBirth = new DateTime(2000, 5, 15),
                        StudentNumber = "S2024001"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student2User.Id,
                        Name = "Bob O'Connor",
                        Email = student2Email,
                        Phone = "087-333-4444",
                        Address = "25 High Street, Cork",
                        DateOfBirth = new DateTime(2001, 8, 20),
                        StudentNumber = "S2024002"
                    }
                };
                context.StudentProfiles.AddRange(students);
                await context.SaveChangesAsync();
            }
        }

        // Seed Courses
        if (!context.Courses.Any())
        {
            var branch = context.Branches.First();
            var faculty = context.FacultyProfiles.First();

            var courses = new[]
            {
                new Course
                {
                    Name = "Computer Science BSc",
                    BranchId = branch.Id,
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2025, 6, 30),
                    FacultyProfileId = faculty.Id
                },
                new Course
                {
                    Name = "Business Studies BA",
                    BranchId = branch.Id,
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2025, 6, 30),
                    FacultyProfileId = faculty.Id
                },
                new Course
                {
                    Name = "Engineering BEng",
                    BranchId = context.Branches.Skip(1).First().Id,
                    StartDate = new DateTime(2024, 9, 1),
                    EndDate = new DateTime(2025, 6, 30),
                    FacultyProfileId = faculty.Id
                }
            };
            context.Courses.AddRange(courses);
            await context.SaveChangesAsync();
        }

        // Seed Enrolments
        if (!context.CourseEnrolments.Any())
        {
            var students = context.StudentProfiles.ToList();
            var courses = context.Courses.ToList();

            var enrolments = new[]
            {
                new CourseEnrolment
                {
                    StudentProfileId = students[0].Id,
                    CourseId = courses[0].Id,
                    EnrolDate = new DateTime(2024, 9, 1),
                    Status = "Active"
                },
                new CourseEnrolment
                {
                    StudentProfileId = students[1].Id,
                    CourseId = courses[0].Id,
                    EnrolDate = new DateTime(2024, 9, 1),
                    Status = "Active"
                },
                new CourseEnrolment
                {
                    StudentProfileId = students[0].Id,
                    CourseId = courses[1].Id,
                    EnrolDate = new DateTime(2024, 9, 1),
                    Status = "Active"
                }
            };
            context.CourseEnrolments.AddRange(enrolments);
            await context.SaveChangesAsync();
        }

        // Seed Attendance
        if (!context.AttendanceRecords.Any())
        {
            var enrolment = context.CourseEnrolments.First();
            var attendanceRecords = new[]
            {
                new AttendanceRecord { CourseEnrolmentId = enrolment.Id, WeekNumber = 1, Date = new DateTime(2024, 9, 2), Present = true },
                new AttendanceRecord { CourseEnrolmentId = enrolment.Id, WeekNumber = 2, Date = new DateTime(2024, 9, 9), Present = true },
                new AttendanceRecord { CourseEnrolmentId = enrolment.Id, WeekNumber = 3, Date = new DateTime(2024, 9, 16), Present = false },
                new AttendanceRecord { CourseEnrolmentId = enrolment.Id, WeekNumber = 4, Date = new DateTime(2024, 9, 23), Present = true }
            };
            context.AttendanceRecords.AddRange(attendanceRecords);
            await context.SaveChangesAsync();
        }

        // Seed Assignments
        if (!context.Assignments.Any())
        {
            var course = context.Courses.First();
            var assignments = new[]
            {
                new Assignment
                {
                    CourseId = course.Id,
                    Title = "Assignment 1: Introduction to Programming",
                    MaxScore = 100,
                    DueDate = new DateTime(2024, 10, 15)
                },
                new Assignment
                {
                    CourseId = course.Id,
                    Title = "Assignment 2: Data Structures",
                    MaxScore = 100,
                    DueDate = new DateTime(2024, 11, 15)
                }
            };
            context.Assignments.AddRange(assignments);
            await context.SaveChangesAsync();
        }

        // Seed Assignment Results
        if (!context.AssignmentResults.Any())
        {
            var assignment = context.Assignments.First();
            var students = context.StudentProfiles.ToList();

            var results = new[]
            {
                new AssignmentResult
                {
                    AssignmentId = assignment.Id,
                    StudentProfileId = students[0].Id,
                    Score = 85,
                    Feedback = "Excellent work!",
                    SubmittedDate = new DateTime(2024, 10, 14)
                },
                new AssignmentResult
                {
                    AssignmentId = assignment.Id,
                    StudentProfileId = students[1].Id,
                    Score = 72,
                    Feedback = "Good effort, but needs improvement.",
                    SubmittedDate = new DateTime(2024, 10, 15)
                }
            };
            context.AssignmentResults.AddRange(results);
            await context.SaveChangesAsync();
        }

        // Seed Exams
        if (!context.Exams.Any())
        {
            var course = context.Courses.First();
            var exams = new[]
            {
                new Exam
                {
                    CourseId = course.Id,
                    Title = "Mid-Term Exam",
                    Date = new DateTime(2024, 12, 10),
                    MaxScore = 100,
                    ResultsReleased = true
                },
                new Exam
                {
                    CourseId = course.Id,
                    Title = "Final Exam",
                    Date = new DateTime(2025, 6, 15),
                    MaxScore = 100,
                    ResultsReleased = false
                }
            };
            context.Exams.AddRange(exams);
            await context.SaveChangesAsync();
        }

        // Seed Exam Results
        if (!context.ExamResults.Any())
        {
            var exams = context.Exams.ToList();
            var students = context.StudentProfiles.ToList();

            var results = new[]
            {
                new ExamResult
                {
                    ExamId = exams[0].Id,
                    StudentProfileId = students[0].Id,
                    Score = 88,
                    Grade = "A"
                },
                new ExamResult
                {
                    ExamId = exams[0].Id,
                    StudentProfileId = students[1].Id,
                    Score = 75,
                    Grade = "B"
                },
                new ExamResult
                {
                    ExamId = exams[1].Id,
                    StudentProfileId = students[0].Id,
                    Score = 92,
                    Grade = "A+"
                },
                new ExamResult
                {
                    ExamId = exams[1].Id,
                    StudentProfileId = students[1].Id,
                    Score = 68,
                    Grade = "C"
                }
            };
            context.ExamResults.AddRange(results);
            await context.SaveChangesAsync();
        }
    }
}
