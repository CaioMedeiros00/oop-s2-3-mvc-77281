# Acme Global College (VGC) - Student & Course Management System

A comprehensive ASP.NET Core MVC web application for managing students, courses, faculty, and academic progress across multiple college branches in Ireland.

> **Note**: This is a clean implementation of the VGC College system. All legacy library management code has been removed.

## Features

### Core Functionality
- **Student Management**: Registration, enrolment, and profile management
- **Course Management**: Multi-branch course offerings with faculty assignments
- **Attendance Tracking**: Weekly attendance recording for enrolled students
- **Academic Progress**: Assignment and exam results with gradebook functionality
- **Results Release Control**: Provisional vs released exam results visibility

### Role-Based Access Control
- **Admin**: Full system access - manage branches, courses, users, and enrolments
- **Faculty**: View and manage assigned courses, students, and gradebook
- **Student**: View personal information, grades, and attendance (restricted access)

## Technology Stack

- **Framework**: ASP.NET Core MVC (.NET 10)
- **Database**: MySQL with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **Testing**: xUnit with InMemory Database Provider
- **CI/CD**: GitHub Actions

## Project Structure

```
├── College.Domain/           # Domain entities and models
│   ├── Branch.cs
│   ├── Course.cs
│   ├── StudentProfile.cs
│   ├── FacultyProfile.cs
│   ├── CourseEnrolment.cs
│   ├── AttendanceRecord.cs
│   ├── Assignment.cs
│   ├── AssignmentResult.cs
│   ├── Exam.cs
│   └── ExamResult.cs
├── Library.MVC/             # ASP.NET Core MVC Web Application (VGC College)
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   └── Views/
├── College.Tests/           # xUnit Test Project
└── .github/workflows/       # GitHub Actions CI configuration
```

## Setup Instructions

### Prerequisites
- .NET 10 SDK or later
- MySQL Server (or use connection string in appsettings.json)
- Visual Studio 2026 or VS Code

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/CaioMedeiros00/AcmeGlobalCollege
   cd AcmeGlobalCollege
   ```

2. **Configure Database Connection**
   
   Update the connection string in `Library.MVC/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "server=localhost;database=vgc_college;user=root;password=yourpassword"
     }
   }
   ```

3. **Restore NuGet Packages**
   ```bash
   dotnet restore
   ```

4. **Run Database Migrations**
   ```bash
   cd Library.MVC
   dotnet ef database update
   ```
   
   The database will be automatically seeded with sample data on first run.

5. **Run the Application**
   ```bash
   dotnet run --project Library.MVC
   ```
   
   Navigate to `https://localhost:5001` (or the URL shown in console)

### Running Tests

```bash
dotnet test
```

Or run tests with detailed output:
```bash
dotnet test --verbosity detailed
```

## Demo Accounts (Available at Login Page)

The application is seeded with the following demo accounts. These are also displayed on the login page for easy reference.

### Admin Account
- **Email**: `admin@vgc.ie`
- **Password**: `Admin@123`
- **Access**: Full system administration

### Faculty Accounts
- **Email**: `john.smith@vgc.ie`
  - **Password**: `Faculty@123`
  - **Assigned**: BSc Computer Science
  
- **Email**: `sarah.jones@vgc.ie`
  - **Password**: `Faculty@123`
  - **Assigned**: BA Business Management

### Student Accounts
- **Email**: `alice.murphy@student.vgc.ie`
  - **Password**: `Student@123`
  - **Student Number**: VGC001
  - **Enrolled**: BSc Computer Science

- **Email**: `bob.kelly@student.vgc.ie`
  - **Password**: `Student@123`
  - **Student Number**: VGC002
  - **Enrolled**: BSc Computer Science

- **Email**: `charlie.walsh@student.vgc.ie`
  - **Password**: `Student@123`
  - **Student Number**: VGC003
  - **Enrolled**: BA Business Management

## System Features by Role

### Admin Dashboard
- Manage branches (create, edit, delete)
- Manage courses and assign faculty
- Create and manage student/faculty profiles
- Enrol students in courses
- Create assignments and exams
- Release exam results
- View all system data

### Faculty Dashboard
- View assigned courses
- Access student list for assigned courses
- View and manage gradebook (assignment results)
- Access student contact details for tutoring
- Track attendance for enrolled students

### Student Dashboard
- View personal profile
- View course enrolments
- Check attendance records
- View assignment results (gradebook)
- View exam results (only if released by admin)
- Access course information

## Security & Authorization

### Server-Side Authorization
- All controllers enforce role-based authorization using `[Authorize]` attributes
- Data access is filtered by user role (Faculty see only their students, Students see only their data)
- Exam results visibility controlled by `ResultsReleased` flag

### Validation
- Server-side validation using Data Annotations
- Model state validation in controllers
- Client-side validation in views

### Error Handling
- Custom error pages for production environment
- Logging of errors during database seeding
- Friendly error messages for users

## Database Schema

### Core Entities
- **Branch**: College branch locations (Dublin, Cork, Galway)
- **Course**: Programs offered at branches
- **StudentProfile**: Student information linked to Identity users
- **FacultyProfile**: Faculty information linked to Identity users
- **CourseEnrolment**: Student-Course enrollment records
- **AttendanceRecord**: Weekly attendance tracking
- **Assignment**: Course assignments with due dates
- **AssignmentResult**: Student grades and feedback
- **Exam**: Course exams with release control
- **ExamResult**: Student exam scores and grades

### Relationships
- One-to-Many: Branch → Courses
- Many-to-Many: Course ↔ Faculty
- One-to-Many: Course → Enrolments
- One-to-Many: StudentProfile → Enrolments
- One-to-Many: CourseEnrolment → AttendanceRecords
- One-to-Many: Course → Assignments/Exams
- Many-to-One: AssignmentResult/ExamResult → Student

## Design Decisions & Assumptions

### Architecture
- **Domain-Driven Design**: Entities in separate Library.Domain project for reusability
- **Repository Pattern**: DbContext acts as repository with direct EF Core usage
- **MVC Pattern**: Clear separation of concerns with Controllers, Models, and Views

### Data Seeding
- Automatic seeding on application start
- 3 branches representing Irish locations
- Multiple courses across branches
- Pre-configured user accounts for all roles
- Sample attendance, assignments, and exam data

### Exam Results Visibility
- Boolean flag `ResultsReleased` controls visibility
- Students cannot see exam results until admin releases them
- Assignment results are always visible (immediate feedback)

### Attendance Tracking
- Simplified weekly session tracking
- Boolean Present/Absent flag
- Linked to course enrolment (not individual courses)

### Faculty-Student Access
- Faculty can only view students in their assigned courses
- Contact details accessible for tutoring purposes
- Gradebook limited to faculty's assigned courses

## Testing Strategy

### Unit Tests (10 tests)
1. Branch creation
2. Student enrolment in course
3. Exam results release control
4. Assignment creation and grading
5. Attendance tracking
6. Student number uniqueness validation
7. Assignment score validation
8. Faculty-course assignment
9. Exam result visibility rules
10. Course enrolment validation

### Test Coverage
- Domain entity validation
- Business logic (enrolment, grading)
- Authorization rules (results visibility)
- Data integrity (unique constraints)

## Continuous Integration

GitHub Actions workflow automatically:
- Restores dependencies
- Builds in Release configuration
- Runs all unit tests
- Fails if build or tests fail

Workflow file: `.github/workflows/ci.yml`

## Future Enhancements

- Module-level course organization
- Advanced timetable management
- File upload for assignments
- Email notifications for result releases
- Reporting and analytics dashboard
- Student performance tracking
- Automated attendance via QR codes

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is part of an academic assignment for Acme Global College.

## Support

For issues or questions, please open an issue in the GitHub repository.

---

**Project Author**: Caio Medeiros  
**Institution**: Acme Global College (VGC)  
**Year**: 2025
