using College.Domain;
using Microsoft.AspNetCore.Identity;

namespace College.MVC.Data
{
    public static class VgcDbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Seed Roles
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
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Seed Faculty Users
            var faculty1Email = "john.smith@vgc.ie";
            var faculty1User = await userManager.FindByEmailAsync(faculty1Email);
            if (faculty1User == null)
            {
                faculty1User = new IdentityUser
                {
                    UserName = faculty1Email,
                    Email = faculty1Email,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(faculty1User, "Faculty@123");
                await userManager.AddToRoleAsync(faculty1User, "Faculty");
            }

            var faculty2Email = "sarah.jones@vgc.ie";
            var faculty2User = await userManager.FindByEmailAsync(faculty2Email);
            if (faculty2User == null)
            {
                faculty2User = new IdentityUser
                {
                    UserName = faculty2Email,
                    Email = faculty2Email,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(faculty2User, "Faculty@123");
                await userManager.AddToRoleAsync(faculty2User, "Faculty");
            }

            // Seed Student Users
            var student1Email = "alice.murphy@student.vgc.ie";
            var student1User = await userManager.FindByEmailAsync(student1Email);
            if (student1User == null)
            {
                student1User = new IdentityUser
                {
                    UserName = student1Email,
                    Email = student1Email,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student1User, "Student@123");
                await userManager.AddToRoleAsync(student1User, "Student");
            }

            var student2Email = "bob.kelly@student.vgc.ie";
            var student2User = await userManager.FindByEmailAsync(student2Email);
            if (student2User == null)
            {
                student2User = new IdentityUser
                {
                    UserName = student2Email,
                    Email = student2Email,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student2User, "Student@123");
                await userManager.AddToRoleAsync(student2User, "Student");
            }

            var student3Email = "charlie.walsh@student.vgc.ie";
            var student3User = await userManager.FindByEmailAsync(student3Email);
            if (student3User == null)
            {
                student3User = new IdentityUser
                {
                    UserName = student3Email,
                    Email = student3Email,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(student3User, "Student@123");
                await userManager.AddToRoleAsync(student3User, "Student");
            }

            // Seed Branches
            if (!context.Branches.Any())
            {
                var branches = new List<Branch>
                {
                    new Branch { Name = "Dublin City Centre", Address = "123 O'Connell Street, Dublin 1, Ireland" },
                    new Branch { Name = "Cork Campus", Address = "45 Patrick Street, Cork, Ireland" },
                    new Branch { Name = "Galway Campus", Address = "78 Shop Street, Galway, Ireland" }
                };
                context.Branches.AddRange(branches);
                await context.SaveChangesAsync();
            }

            // Seed Faculty Profiles
            if (!context.FacultyProfiles.Any())
            {
                var facultyProfiles = new List<FacultyProfile>
                {
                    new FacultyProfile
                    {
                        IdentityUserId = faculty1User.Id,
                        Name = "Dr. John Smith",
                        Email = faculty1Email,
                        Phone = "+353871234567"
                    },
                    new FacultyProfile
                    {
                        IdentityUserId = faculty2User.Id,
                        Name = "Prof. Sarah Jones",
                        Email = faculty2Email,
                        Phone = "+353871234568"
                    }
                };
                context.FacultyProfiles.AddRange(facultyProfiles);
                await context.SaveChangesAsync();
            }

            // Seed Student Profiles
            if (!context.StudentProfiles.Any())
            {
                var studentProfiles = new List<StudentProfile>
                {
                    new StudentProfile
                    {
                        IdentityUserId = student1User.Id,
                        Name = "Alice Murphy",
                        Email = student1Email,
                        Phone = "+353861111111",
                        Address = "12 Main Street, Dublin 2",
                        DateOfBirth = new DateTime(2002, 5, 15),
                        StudentNumber = "VGC001"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student2User.Id,
                        Name = "Bob Kelly",
                        Email = student2Email,
                        Phone = "+353862222222",
                        Address = "34 High Street, Cork",
                        DateOfBirth = new DateTime(2003, 8, 22),
                        StudentNumber = "VGC002"
                    },
                    new StudentProfile
                    {
                        IdentityUserId = student3User.Id,
                        Name = "Charlie Walsh",
                        Email = student3Email,
                        Phone = "+353863333333",
                        Address = "56 Market Square, Galway",
                        DateOfBirth = new DateTime(2001, 11, 30),
                        StudentNumber = "VGC003"
                    }
                };
                context.StudentProfiles.AddRange(studentProfiles);
                await context.SaveChangesAsync();
            }

            // Seed Courses
            if (!context.Courses.Any())
            {
                var branch1 = context.Branches.First(b => b.Name == "Dublin City Centre");
                var branch2 = context.Branches.First(b => b.Name == "Cork Campus");
                var branch3 = context.Branches.First(b => b.Name == "Galway Campus");

                var courses = new List<Course>
                {
                    new Course
                    {
                        Name = "BSc Computer Science",
                        BranchId = branch1.Id,
                        StartDate = new DateTime(2025, 9, 1),
                        EndDate = new DateTime(2026, 6, 30)
                    },
                    new Course
                    {
                        Name = "BA Business Management",
                        BranchId = branch1.Id,
                        StartDate = new DateTime(2025, 9, 1),
                        EndDate = new DateTime(2026, 6, 30)
                    },
                    new Course
                    {
                        Name = "BSc Data Science",
                        BranchId = branch2.Id,
                        StartDate = new DateTime(2025, 9, 1),
                        EndDate = new DateTime(2026, 6, 30)
                    },
                    new Course
                    {
                        Name = "BA Digital Marketing",
                        BranchId = branch3.Id,
                        StartDate = new DateTime(2025, 9, 1),
                        EndDate = new DateTime(2026, 6, 30)
                    }
                };
                context.Courses.AddRange(courses);
                await context.SaveChangesAsync();

                // Assign Faculty to Courses
                var faculty1 = context.FacultyProfiles.First(f => f.Email == faculty1Email);
                var faculty2 = context.FacultyProfiles.First(f => f.Email == faculty2Email);

                var course1 = context.Courses.First(c => c.Name == "BSc Computer Science");
                var course2 = context.Courses.First(c => c.Name == "BA Business Management");

                course1.FacultyMembers.Add(faculty1);
                course2.FacultyMembers.Add(faculty2);
                await context.SaveChangesAsync();
            }

            // Seed Enrolments
            if (!context.CourseEnrolments.Any())
            {
                var student1 = context.StudentProfiles.First(s => s.StudentNumber == "VGC001");
                var student2 = context.StudentProfiles.First(s => s.StudentNumber == "VGC002");
                var student3 = context.StudentProfiles.First(s => s.StudentNumber == "VGC003");

                var course1 = context.Courses.First(c => c.Name == "BSc Computer Science");
                var course2 = context.Courses.First(c => c.Name == "BA Business Management");

                var enrolments = new List<CourseEnrolment>
                {
                    new CourseEnrolment
                    {
                        StudentProfileId = student1.Id,
                        CourseId = course1.Id,
                        EnrolDate = new DateTime(2025, 9, 1),
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = student2.Id,
                        CourseId = course1.Id,
                        EnrolDate = new DateTime(2025, 9, 1),
                        Status = "Active"
                    },
                    new CourseEnrolment
                    {
                        StudentProfileId = student3.Id,
                        CourseId = course2.Id,
                        EnrolDate = new DateTime(2025, 9, 1),
                        Status = "Active"
                    }
                };
                context.CourseEnrolments.AddRange(enrolments);
                await context.SaveChangesAsync();
            }

            // Seed Attendance Records
            if (!context.AttendanceRecords.Any())
            {
                var enrolment1 = context.CourseEnrolments.First();
                var attendanceRecords = new List<AttendanceRecord>
                {
                    new AttendanceRecord { CourseEnrolmentId = enrolment1.Id, WeekNumber = 1, Date = new DateTime(2025, 9, 8), Present = true },
                    new AttendanceRecord { CourseEnrolmentId = enrolment1.Id, WeekNumber = 2, Date = new DateTime(2025, 9, 15), Present = true },
                    new AttendanceRecord { CourseEnrolmentId = enrolment1.Id, WeekNumber = 3, Date = new DateTime(2025, 9, 22), Present = false },
                    new AttendanceRecord { CourseEnrolmentId = enrolment1.Id, WeekNumber = 4, Date = new DateTime(2025, 9, 29), Present = true }
                };
                context.AttendanceRecords.AddRange(attendanceRecords);
                await context.SaveChangesAsync();
            }

            // Seed Assignments
            if (!context.Assignments.Any())
            {
                var course1 = context.Courses.First(c => c.Name == "BSc Computer Science");
                var course2 = context.Courses.First(c => c.Name == "BA Business Management");

                var assignments = new List<Assignment>
                {
                    new Assignment
                    {
                        CourseId = course1.Id,
                        Title = "Data Structures - Assignment 1",
                        MaxScore = 100,
                        DueDate = new DateTime(2025, 10, 15)
                    },
                    new Assignment
                    {
                        CourseId = course1.Id,
                        Title = "Algorithms - Assignment 2",
                        MaxScore = 100,
                        DueDate = new DateTime(2025, 11, 20)
                    },
                    new Assignment
                    {
                        CourseId = course2.Id,
                        Title = "Marketing Strategy Project",
                        MaxScore = 100,
                        DueDate = new DateTime(2025, 10, 30)
                    }
                };
                context.Assignments.AddRange(assignments);
                await context.SaveChangesAsync();
            }

            // Seed Assignment Results
            if (!context.AssignmentResults.Any())
            {
                var student1 = context.StudentProfiles.First(s => s.StudentNumber == "VGC001");
                var student2 = context.StudentProfiles.First(s => s.StudentNumber == "VGC002");
                var assignment1 = context.Assignments.First(a => a.Title == "Data Structures - Assignment 1");

                var assignmentResults = new List<AssignmentResult>
                {
                    new AssignmentResult
                    {
                        AssignmentId = assignment1.Id,
                        StudentProfileId = student1.Id,
                        Score = 85,
                        Feedback = "Excellent work! Good understanding of concepts."
                    },
                    new AssignmentResult
                    {
                        AssignmentId = assignment1.Id,
                        StudentProfileId = student2.Id,
                        Score = 72,
                        Feedback = "Good effort. Review binary trees section."
                    }
                };
                context.AssignmentResults.AddRange(assignmentResults);
                await context.SaveChangesAsync();
            }

            // Seed Exams
            if (!context.Exams.Any())
            {
                var course1 = context.Courses.First(c => c.Name == "BSc Computer Science");
                var course2 = context.Courses.First(c => c.Name == "BA Business Management");

                var exams = new List<Exam>
                {
                    new Exam
                    {
                        CourseId = course1.Id,
                        Title = "Midterm Exam - Data Structures",
                        Date = new DateTime(2025, 12, 10),
                        MaxScore = 100,
                        ResultsReleased = true
                    },
                    new Exam
                    {
                        CourseId = course1.Id,
                        Title = "Final Exam - Computer Science",
                        Date = new DateTime(2026, 6, 15),
                        MaxScore = 100,
                        ResultsReleased = false
                    },
                    new Exam
                    {
                        CourseId = course2.Id,
                        Title = "Business Management Final",
                        Date = new DateTime(2026, 6, 20),
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
                var student1 = context.StudentProfiles.First(s => s.StudentNumber == "VGC001");
                var student2 = context.StudentProfiles.First(s => s.StudentNumber == "VGC002");
                var exam1 = context.Exams.First(e => e.Title == "Midterm Exam - Data Structures");
                var exam2 = context.Exams.First(e => e.Title == "Final Exam - Computer Science");

                var examResults = new List<ExamResult>
                {
                    new ExamResult
                    {
                        ExamId = exam1.Id,
                        StudentProfileId = student1.Id,
                        Score = 88,
                        Grade = "A"
                    },
                    new ExamResult
                    {
                        ExamId = exam1.Id,
                        StudentProfileId = student2.Id,
                        Score = 75,
                        Grade = "B"
                    },
                    new ExamResult
                    {
                        ExamId = exam2.Id,
                        StudentProfileId = student1.Id,
                        Score = 92,
                        Grade = "A"
                    },
                    new ExamResult
                    {
                        ExamId = exam2.Id,
                        StudentProfileId = student2.Id,
                        Score = 68,
                        Grade = "C"
                    }
                };
                context.ExamResults.AddRange(examResults);
                await context.SaveChangesAsync();
            }
        }
    }
}
