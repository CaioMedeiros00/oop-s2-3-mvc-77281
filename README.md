# Acme Global College - Student Management System

ASP.NET Core MVC application for managing students, courses, faculty, and academic progress.

## Setup

1. Clone the repository
2. Run: dotnet restore
3. Run: dotnet run --project College.MVC
4. Navigate to https://localhost:5001

## Demo Accounts

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@vgc.ie | Admin123! |
| Faculty | faculty@vgc.ie | Faculty123! |
| Student 1 | student1@vgc.ie | Student123! |
| Student 2 | student2@vgc.ie | Student123! |

## Run Tests

dotnet test

## Features

- Branch and Course Management
- Student Enrolment and Attendance Tracking
- Assignment and Exam Management
- Role-Based Access Control
- Automatic Database Seeding

## Technology

- .NET 10
- Entity Framework Core
- SQLite
- ASP.NET Core Identity
- xUnit Tests
