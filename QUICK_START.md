# VGC College - Quick Start Guide

## 🚀 Quick Start

### 1. Setup Database
```bash
cd Library.MVC
dotnet ef database update
```

### 2. Run Application
```bash
dotnet run --project Library.MVC
```

### 3. Access Application
Navigate to: `https://localhost:5001`

---

## 🔑 Demo Accounts

### Admin Account
**Email**: `admin@vgc.ie`  
**Password**: `Admin@123`

**Capabilities**:
- Manage all branches, courses, and users
- Create and manage enrolments
- Create assignments and exams
- Release exam results
- View all data across the system

---

### Faculty Accounts

#### Faculty 1 - Dr. John Smith
**Email**: `john.smith@vgc.ie`  
**Password**: `Faculty@123`  
**Assigned Course**: BSc Computer Science

**Capabilities**:
- View students in assigned courses
- Manage gradebook for assignments
- View student contact details
- Track attendance

#### Faculty 2 - Prof. Sarah Jones
**Email**: `sarah.jones@vgc.ie`  
**Password**: `Faculty@123`  
**Assigned Course**: BA Business Management

**Capabilities**:
- View students in assigned courses
- Manage gradebook for assignments
- View student contact details
- Track attendance

---

### Student Accounts

#### Student 1 - Alice Murphy
**Email**: `alice.murphy@student.vgc.ie`  
**Password**: `Student@123`  
**Student Number**: VGC001  
**Enrolled In**: BSc Computer Science

**Can View**:
- Personal profile and enrolment information
- Course details
- Attendance records
- Assignment results (all grades)
- Exam results (only if released by admin)

#### Student 2 - Bob Kelly
**Email**: `bob.kelly@student.vgc.ie`  
**Password**: `Student@123`  
**Student Number**: VGC002  
**Enrolled In**: BSc Computer Science

**Can View**:
- Personal profile and enrolment information
- Course details
- Attendance records
- Assignment results (all grades)
- Exam results (only if released by admin)

#### Student 3 - Charlie Walsh
**Email**: `charlie.walsh@student.vgc.ie`  
**Password**: `Student@123`  
**Student Number**: VGC003  
**Enrolled In**: BA Business Management

**Can View**:
- Personal profile and enrolment information
- Course details
- Attendance records
- Assignment results (all grades)
- Exam results (only if released by admin)

---

## 📊 Seeded Data Overview

### Branches (3)
1. Dublin City Centre - 123 O'Connell Street, Dublin 1
2. Cork Campus - 45 Patrick Street, Cork
3. Galway Campus - 78 Shop Street, Galway

### Courses (4)
1. **BSc Computer Science** (Dublin) - 2025/09/01 to 2026/06/30
2. **BA Business Management** (Dublin) - 2025/09/01 to 2026/06/30
3. **BSc Data Science** (Cork) - 2025/09/01 to 2026/06/30
4. **BA Digital Marketing** (Galway) - 2025/09/01 to 2026/06/30

### Sample Data
- **Enrolments**: 3 active student enrolments
- **Assignments**: 3 assignments across courses
- **Assignment Results**: Sample grades for students
- **Exams**: 3 exams (1 released, 2 provisional)
- **Exam Results**: Sample exam grades
- **Attendance**: Sample weekly attendance records

---

## 🧪 Running Tests

### Run all tests
```bash
dotnet test
```

### Run with detailed output
```bash
dotnet test --verbosity detailed
```

### Expected Results
- **Total Tests**: 17 (10 VGC + 7 Library)
- **Expected**: All Pass ✅

---

## 🎯 Testing Different Roles

### Test as Admin
1. Login with `admin@vgc.ie` / `Admin@123`
2. Navigate to:
   - `/Branches` - Manage branches
   - `/Courses` - Manage courses
   - `/Enrolments` - Manage student enrolments
   - `/Assignments` - Create assignments
   - `/Exams` - Create exams and release results

### Test as Faculty
1. Login with `john.smith@vgc.ie` / `Faculty@123`
2. Navigate to:
   - `/FacultyDashboard` - View dashboard
   - `/FacultyDashboard/MyCourses` - View assigned courses
   - `/FacultyDashboard/Students` - View students in courses
   - `/FacultyDashboard/Gradebook` - View/manage grades

### Test as Student
1. Login with `alice.murphy@student.vgc.ie` / `Student@123`
2. Navigate to:
   - `/StudentDashboard` - View personal dashboard
   - `/StudentDashboard/Enrolments` - View course enrolments
   - `/StudentDashboard/Grades` - View assignment and exam results
   - `/StudentDashboard/Attendance` - View attendance records

---

## 🔒 Testing Authorization

### Exam Results Visibility Test
1. Login as **Admin** (`admin@vgc.ie`)
2. Navigate to `/Exams`
3. Find "Final Exam - Computer Science" (ResultsReleased = false)
4. Logout and login as **Student** (`alice.murphy@student.vgc.ie`)
5. Navigate to `/StudentDashboard/Grades`
6. **Verify**: Final exam results are NOT visible
7. Logout and login as **Admin** again
8. Release the results (click "Release Results")
9. Logout and login as **Student** again
10. **Verify**: Final exam results are NOW visible

### Faculty Access Control Test
1. Login as **Faculty** (`john.smith@vgc.ie`)
2. Navigate to `/FacultyDashboard/Students`
3. **Verify**: Can only see students enrolled in BSc Computer Science
4. **Verify**: Cannot see students from BA Business Management

### Student Access Control Test
1. Login as **Student** (`alice.murphy@student.vgc.ie`)
2. Try to navigate to `/StudentDashboard/Grades`
3. **Verify**: Can only see own grades
4. **Verify**: Cannot access other students' information

---

## 📝 Common Tasks

### Create a New Student Enrolment (Admin)
1. Login as Admin
2. Go to `/Enrolments`
3. Click "Create New"
4. Select Student and Course
5. Submit

### Add an Assignment Result (Admin)
1. Login as Admin
2. Navigate to database or create controller action
3. Add AssignmentResult with StudentId, AssignmentId, Score, Feedback

### Release Exam Results (Admin)
1. Login as Admin
2. Go to `/Exams`
3. Find the exam
4. Click "Release Results"
5. Students can now see their exam grades

---

## 🛠️ Troubleshooting

### Database Issues
```bash
# Drop and recreate database
dotnet ef database drop --project Library.MVC
dotnet ef database update --project Library.MVC
```

### Migration Issues
```bash
# Remove last migration
dotnet ef migrations remove --project Library.MVC

# Add new migration
dotnet ef migrations add MigrationName --project Library.MVC
```

### Build Issues
```bash
# Clean and rebuild
dotnet clean
dotnet build
```

---

## 📚 Additional Resources

- **Full Documentation**: See `README.md`
- **Implementation Details**: See `VGC_IMPLEMENTATION_SUMMARY.md`
- **Files Created**: See `FILES_CREATED.md`

---

## ✅ Verification Checklist

- [ ] Database migrated successfully
- [ ] Application runs without errors
- [ ] Can login as Admin
- [ ] Can login as Faculty
- [ ] Can login as Student
- [ ] All tests pass (17/17)
- [ ] Admin can create branches
- [ ] Admin can create courses
- [ ] Admin can manage enrolments
- [ ] Faculty can view their students
- [ ] Students can view their grades
- [ ] Exam results visibility works correctly

---

**Quick Reference**: Keep this guide handy while developing and testing the VGC College application!
