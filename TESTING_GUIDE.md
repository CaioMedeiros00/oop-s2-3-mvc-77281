# Quick Testing Guide for VGC College

## How to Test Your Application

### 1. Run the Application

```bash
cd Library.MVC
dotnet run
```

Navigate to: `https://localhost:5001` or `http://localhost:5000`

---

## 2. Test Each Role

### **Admin Role** (`admin@vgc.ie` / `Admin@123`)

#### Test Branches Management:
1. Click "Branches" in nav bar
2. Create a new branch (e.g., "Limerick Campus")
3. Edit an existing branch
4. View the list

#### Test Courses Management:
1. Click "Courses" in nav bar
2. Create a new course (select a branch)
3. View course details (should show faculty members)
4. Edit/Delete a course

#### Test Enrolments:
1. Click "Enrolments"
2. Create new enrolment (select student + course)
3. View all enrolments

#### Test Assignments:
1. Click "Assignments"
2. Create new assignment for a course
3. View assignment list

#### Test Exams:
1. Click "Exams"
2. Create new exam (leave "Results Released" unchecked)
3. Click "Release Results" button to release
4. Verify status changes from "Provisional" to "Released"

---

### **Faculty Role** (`john.smith@vgc.ie` / `Faculty@123`)

#### Test Faculty Dashboard:
1. Login as faculty
2. Should see dashboard with assigned courses
3. Click "My Courses" - should see "BSc Computer Science"

#### Test Students List:
1. Click "Students" in nav bar
2. Should ONLY see students in assigned courses
3. Verify you can see student contact details (email, phone)

#### Test Gradebook:
1. Click "Gradebook"
2. Should see assignment results for your courses only
3. Filter by course dropdown

**Important**: Faculty should NOT see:
- Students from other courses
- Branches/Courses management
- Admin controls

---

### **Student Role** (`alice.murphy@student.vgc.ie` / `Student@123`)

#### Test Student Dashboard:
1. Login as student
2. View dashboard (shows enrolments count, results count)
3. View profile information

#### Test My Courses:
1. Click "My Courses" / "Enrolments"
2. Should see enrolled course: "BSc Computer Science"
3. Check attendance percentage

#### Test Grades:
1. Click "Grades"
2. View assignment results
3. **CRITICAL TEST**: Exam results section should show:
   - "Released Only" in the header
   - Only exams where `ResultsReleased = true`
   - If no results released: "No exam results have been released yet"

#### Test Attendance:
1. Click "Attendance"
2. View attendance records grouped by course
3. See overall attendance percentage

**Important**: Students should NOT see:
- Provisional exam results
- Other students' data
- Admin/Faculty features

---

## 3. Test Result Release Control (KEY REQUIREMENT!)

### Scenario: Exam Results Visibility

1. **As Admin:**
   - Create an exam with `ResultsReleased = false`
   - Note the exam ID

2. **As Student:**
   - Go to "Grades"
   - Verify the exam result does NOT appear in the list
   - (Or shows message: "No exam results have been released yet")

3. **As Admin:**
   - Go to "Exams"
   - Find the exam
   - Click "Release Results" button
   - Verify badge changes to "Released" (green)

4. **As Student:**
   - Refresh "Grades" page
   - **NOW** the exam result should appear
   - ✅ **This proves the ResultsReleased flag works!**

---

## 4. Test Authorization (RBAC)

### Test Server-Side Enforcement:

1. **As Student**, try to access admin URLs directly:
   ```
   https://localhost:5001/Branches
   https://localhost:5001/Courses
   https://localhost:5001/Admin/Roles
   ```
   **Expected**: HTTP 403 Forbidden or redirect to Access Denied

2. **As Faculty**, try to access student-specific URLs:
   ```
   https://localhost:5001/StudentDashboard
   ```
   **Expected**: HTTP 403 Forbidden

3. **As Student**, try to access faculty URLs:
   ```
   https://localhost:5001/FacultyDashboard
   ```
   **Expected**: HTTP 403 Forbidden

✅ If you get access denied errors, authorization is working correctly!

---

## 5. Test Validation

1. **Create Branch** with empty name
   - Expected: Validation error "The Name field is required"

2. **Create Course** with EndDate before StartDate
   - Expected: Should accept (no validation), but best practice would validate

3. **Create Enrolment** without selecting student
   - Expected: Validation error

---

## 6. Run All Tests

```bash
dotnet test
```

**Expected Output:**
```
Passed! - 10 tests passed, 0 tests failed
```

All 10 tests should pass:
- ✅ CanCreateBranch
- ✅ CanEnrolStudentInCourse
- ✅ ExamResultsNotVisibleUntilReleased
- ✅ CanReleaseExamResults
- ✅ CanCreateAssignmentForCourse
- ✅ CanRecordAssignmentResults
- ✅ CanTrackAttendance
- ✅ StudentNumberShouldBeUnique
- ✅ AssignmentScoreMustBeValid
- ✅ CanAssignFacultyToCourse

---

## 7. Verify CI Build on GitHub

1. Push your code to GitHub
2. Go to repository → Actions tab
3. Verify latest workflow run shows: ✅ **Success**

---

## ✅ CHECKLIST FOR MARKER

When the lecturer/marker tests your assignment, they will check:

- [x] Can login with provided demo accounts
- [x] Admin can manage all entities (CRUD)
- [x] Faculty sees only their courses and students
- [x] Students see only their own data
- [x] Exam results hidden until released
- [x] Navigation changes based on role
- [x] Authorization enforced (server-side, not just UI)
- [x] All 10 tests pass
- [x] CI build succeeds on GitHub
- [x] README has accurate setup instructions

---

## 🎯 PASS CRITERIA

If ALL of the above works, you meet **100% of the requirements** and should receive **FULL MARKS**.

---

## 📝 TROUBLESHOOTING

### Database Issues?
```bash
cd Library.MVC
dotnet ef database drop
dotnet ef database update
```
Restart the app - seed data will recreate.

### Views Not Found?
- Ensure you're running **Library.MVC** (not College.MVC)
- Check views are in correct folders under `Library.MVC\Views\`

### Login Issues?
- Passwords are case-sensitive
- Default accounts are in README.md
- Check database has seed data

---

## 🏁 FINAL CHECK

Before submission, verify:
1. ✅ App runs locally without errors
2. ✅ All 3 roles can login
3. ✅ Exam results release control works
4. ✅ All tests pass
5. ✅ GitHub Actions CI build succeeds
6. ✅ README has demo credentials

**If all checked, you're ready to submit! Good luck! 🎓**
