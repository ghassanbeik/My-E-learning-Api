using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Infrastructure.Data;

namespace Horizon.Infrastructure.Seeding;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await SeedRolesAsync(context);
        await SeedPermissionsAsync(context);
        await SeedUsersAsync(context);
        await SeedCategoriesAsync(context);
        await SeedTagsAsync(context);
        await SeedCoursesAsync(context);
        await SeedEnrollmentsAndProgressAsync(context);
        await SeedReviewsAsync(context);
        await SeedCouponsAsync(context);
        await SeedNotificationsAsync(context);
    }

    // ─── Roles ──────────────────────────────────────────────────────────────

    private static async Task SeedRolesAsync(ApplicationDbContext context)
    {
        if (await context.Roles.AnyAsync()) return;

        var roles = new List<RoleInfo>
        {
            new() { Id = RoleIds.Admin,      Name = "Admin",      Description = "Platform administrator with full access" },
            new() { Id = RoleIds.Instructor, Name = "Instructor", Description = "Course instructor who can create and manage courses" },
            new() { Id = RoleIds.Student,    Name = "Student",    Description = "Learner who can enroll and take courses" },
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }

    // ─── Permissions ────────────────────────────────────────────────────────

    private static async Task SeedPermissionsAsync(ApplicationDbContext context)
    {
        if (await context.Permissions.AnyAsync()) return;

        var permissions = new List<Permission>
        {
            // Course permissions
            new() { Id = Guid.NewGuid(), Name = "Create Course",  Resource = "Course", Action = "Create" },
            new() { Id = Guid.NewGuid(), Name = "Edit Course",    Resource = "Course", Action = "Edit"   },
            new() { Id = Guid.NewGuid(), Name = "Delete Course",  Resource = "Course", Action = "Delete" },
            new() { Id = Guid.NewGuid(), Name = "Publish Course", Resource = "Course", Action = "Publish"},
            // User permissions
            new() { Id = Guid.NewGuid(), Name = "Manage Users",   Resource = "User",   Action = "Manage" },
            new() { Id = Guid.NewGuid(), Name = "Ban Users",      Resource = "User",   Action = "Ban"    },
            // Payout permissions
            new() { Id = Guid.NewGuid(), Name = "Approve Payout", Resource = "Payout", Action = "Approve"},
            // Coupon permissions
            new() { Id = Guid.NewGuid(), Name = "Manage Coupons", Resource = "Coupon", Action = "Manage" },
            // Review permissions
            new() { Id = Guid.NewGuid(), Name = "Moderate Reviews", Resource = "Review", Action = "Moderate" },
        };

        await context.Permissions.AddRangeAsync(permissions);
        await context.SaveChangesAsync();
    }

    // ─── Users ──────────────────────────────────────────────────────────────

    private static async Task SeedUsersAsync(ApplicationDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        // Admin user
        var admin = new UserInfo
        {
            Id             = UserIds.Admin,
            Email          = "admin@elearning.com",
            PasswordHash   = BCryptHash("Admin@123456"),
            FirstName      = "Platform",
            LastName       = "Admin",
            IsEmailVerified = true,
            IsActive       = true,
            Headline       = "Platform Administrator",
        };

        // Instructor 1
        var instructor1 = new UserInfo
        {
            Id             = UserIds.Instructor1,
            Email          = "john.instructor@elearning.com",
            PasswordHash   = BCryptHash("Instructor@123"),
            FirstName      = "John",
            LastName       = "Smith",
            IsEmailVerified = true,
            IsActive       = true,
            Headline       = "Senior Software Engineer & Educator",
            Bio            = "10 years of experience in software development. Passionate about teaching clean code and architecture.",
            Website        = "https://johnsmith.dev",
            LinkedIn       = "https://linkedin.com/in/johnsmith",
        };

        // Instructor 2
        var instructor2 = new UserInfo
        {
            Id             = UserIds.Instructor2,
            Email          = "sarah.instructor@elearning.com",
            PasswordHash   = BCryptHash("Instructor@123"),
            FirstName      = "Sarah",
            LastName       = "Johnson",
            IsEmailVerified = true,
            IsActive       = true,
            Headline       = "Full Stack Developer & UI/UX Enthusiast",
            Bio            = "Frontend specialist with 7 years building modern web apps.",
            LinkedIn       = "https://linkedin.com/in/sarahjohnson",
        };

        // Students
        var students = new List<UserInfo>
        {
            new() { Id = UserIds.Student1, Email = "alice@student.com", PasswordHash = BCryptHash("Student@123"), FirstName = "Alice",   LastName = "Brown",   IsEmailVerified = true, IsActive = true },
            new() { Id = UserIds.Student2, Email = "bob@student.com",   PasswordHash = BCryptHash("Student@123"), FirstName = "Bob",     LastName = "Davis",   IsEmailVerified = true, IsActive = true },
            new() { Id = UserIds.Student3, Email = "carol@student.com", PasswordHash = BCryptHash("Student@123"), FirstName = "Carol",   LastName = "Wilson",  IsEmailVerified = true, IsActive = true },
            new() { Id = UserIds.Student4, Email = "dave@student.com",  PasswordHash = BCryptHash("Student@123"), FirstName = "Dave",    LastName = "Martinez",IsEmailVerified = true, IsActive = true },
            new() { Id = UserIds.Student5, Email = "eve@student.com",   PasswordHash = BCryptHash("Student@123"), FirstName = "Eve",     LastName = "Taylor",  IsEmailVerified = true, IsActive = true },
        };

        await context.Users.AddRangeAsync(new[] { admin, instructor1, instructor2 }.Concat(students));
        await context.SaveChangesAsync();

        // Assign roles
        var userRoles = new List<UserRole>
        {
            new() { Id = Guid.NewGuid(), UserId = UserIds.Admin,       RoleId = RoleIds.Admin      },
            new() { Id = Guid.NewGuid(), UserId = UserIds.Admin,       RoleId = RoleIds.Instructor },
            new() { Id = Guid.NewGuid(), UserId = UserIds.Instructor1, RoleId = RoleIds.Instructor },
            new() { Id = Guid.NewGuid(), UserId = UserIds.Instructor2, RoleId = RoleIds.Instructor },
            new() { Id = Guid.NewGuid(), UserId = UserIds.Student1,    RoleId = RoleIds.Student    },
            new() { Id = Guid.NewGuid(), UserId = UserIds.Student2,    RoleId = RoleIds.Student    },
            new() { Id = Guid.NewGuid(), UserId = UserIds.Student3,    RoleId = RoleIds.Student    },
            new() { Id = Guid.NewGuid(), UserId = UserIds.Student4,    RoleId = RoleIds.Student    },
            new() { Id = Guid.NewGuid(), UserId = UserIds.Student5,    RoleId = RoleIds.Student    },
        };

        await context.UserRoles.AddRangeAsync(userRoles);

        // Instructor profiles
        var profiles = new List<InstructorProfile>
        {
            new()
            {
                Id             = Guid.NewGuid(),
                UserId         = UserIds.Instructor1,
                TeachingExperience = "10 years in software development and 5 years of teaching online",
                Education      = "B.Sc. Computer Science, MIT",
                AverageRating  = 4.8m,
                TotalStudents  = 12500,
                TotalCourses   = 5,
                IsVerified     = true,
                TotalEarnings  = 85000m,
                PendingEarnings = 3200m,
            },
            new()
            {
                Id             = Guid.NewGuid(),
                UserId         = UserIds.Instructor2,
                TeachingExperience = "7 years in full stack development",
                Education      = "B.Sc. Information Systems, Stanford",
                AverageRating  = 4.6m,
                TotalStudents  = 8700,
                TotalCourses   = 3,
                IsVerified     = true,
                TotalEarnings  = 52000m,
                PendingEarnings = 1800m,
            },
        };

        await context.InstructorProfiles.AddRangeAsync(profiles);
        await context.SaveChangesAsync();
    }

    // ─── Categories ─────────────────────────────────────────────────────────

    private static async Task SeedCategoriesAsync(ApplicationDbContext context)
    {
        if (await context.Categories.AnyAsync()) return;

        var development = new Category { Id = CategoryIds.Development, Name = "Development",    Description = "Software development and programming courses", IsFeatured = true, DisplayOrder = 1 };
        var design      = new Category { Id = CategoryIds.Design,       Name = "Design",         Description = "UI/UX, graphic design, and creative arts",       IsFeatured = true, DisplayOrder = 2 };
        var business    = new Category { Id = CategoryIds.Business,     Name = "Business",       Description = "Entrepreneurship, management and finance",        IsFeatured = true, DisplayOrder = 3 };
        var marketing   = new Category { Id = CategoryIds.Marketing,    Name = "Marketing",      Description = "Digital marketing, SEO and social media",         IsFeatured = false, DisplayOrder = 4 };

        await context.Categories.AddRangeAsync(development, design, business, marketing);
        await context.SaveChangesAsync();

        // Sub-categories
        var subCategories = new List<Category>
        {
            new() { Id = CategoryIds.WebDev,    Name = "Web Development",    ParentId = CategoryIds.Development, DisplayOrder = 1 },
            new() { Id = CategoryIds.MobileDev, Name = "Mobile Development", ParentId = CategoryIds.Development, DisplayOrder = 2 },
            new() { Id = CategoryIds.DataSci,   Name = "Data Science",       ParentId = CategoryIds.Development, DisplayOrder = 3 },
            new() { Id = CategoryIds.UIDesign,  Name = "UI/UX Design",       ParentId = CategoryIds.Design,      DisplayOrder = 1 },
            new() { Id = CategoryIds.GraphicDes,Name = "Graphic Design",     ParentId = CategoryIds.Design,      DisplayOrder = 2 },
        };

        await context.Categories.AddRangeAsync(subCategories);
        await context.SaveChangesAsync();
    }

    // ─── Tags ───────────────────────────────────────────────────────────────

    private static async Task SeedTagsAsync(ApplicationDbContext context)
    {
        if (await context.Tags.AnyAsync()) return;

        var tags = new List<Tag>
        {
            new() { Id = TagIds.CSharp,      Name = "C#",         UsageCount = 45 },
            new() { Id = TagIds.DotNet,      Name = ".NET",       UsageCount = 38 },
            new() { Id = TagIds.React,       Name = "React",      UsageCount = 62 },
            new() { Id = TagIds.JavaScript,  Name = "JavaScript", UsageCount = 80 },
            new() { Id = TagIds.TypeScript,  Name = "TypeScript", UsageCount = 55 },
            new() { Id = TagIds.Python,      Name = "Python",     UsageCount = 90 },
            new() { Id = TagIds.Docker,      Name = "Docker",     UsageCount = 30 },
            new() { Id = TagIds.Azure,       Name = "Azure",      UsageCount = 25 },
            new() { Id = TagIds.CleanCode,   Name = "Clean Code", UsageCount = 40 },
            new() { Id = TagIds.API,         Name = "REST API",   UsageCount = 50 },
        };

        await context.Tags.AddRangeAsync(tags);
        await context.SaveChangesAsync();
    }

    // ─── Courses ────────────────────────────────────────────────────────────

    private static async Task SeedCoursesAsync(ApplicationDbContext context)
    {
        if (await context.Courses.AnyAsync()) return;

        var course1 = new Course
        {
            Id               = CourseIds.Course1,
            InstructorId     = UserIds.Instructor1,
            Title            = "Complete ASP.NET Core Web API Development",
            Subtitle         = "Build production-ready REST APIs with .NET 8, EF Core, Clean Architecture and more",
            Description      = "Master ASP.NET Core Web API development from the ground up. This comprehensive course covers everything you need to build professional, scalable, and maintainable REST APIs. You'll learn Clean Architecture, CQRS with MediatR, Entity Framework Core, JWT authentication, and deployment to Azure.",
            ShortDescription = "Learn to build production-ready ASP.NET Core APIs with Clean Architecture",
            Language         = "English",
            Level            = CourseLevel.Intermediate,
            Status           = CourseStatus.Published,
            Price            = 89.99m,
            DiscountPrice    = 14.99m,
            DiscountExpiry   = DateTime.UtcNow.AddDays(7),
            Currency         = "USD",
            DurationMinutes  = 1440,
            IsFeatured       = true,
            IsLifetimeAccess = true,
            AverageRating    = 4.8,
            TotalReviews     = 1243,
            TotalStudents    = 8750,
            TotalLessons     = 120,
            LearningObjectives = "Build REST APIs with ASP.NET Core|Implement Clean Architecture|Use CQRS and MediatR|JWT Authentication|Deploy to Azure",
            Prerequisites    = "Basic C# knowledge|Understanding of OOP principles",
            TargetAudience   = "Developers who want to master backend development with .NET",
            WelcomeMessage   = "Welcome! Get ready to become a professional API developer.",
            CongratulationMessage = "Congratulations on completing the course! You're now ready to build production APIs.",
        };

        var course2 = new Course
        {
            Id               = CourseIds.Course2,
            InstructorId     = UserIds.Instructor2,
            Title            = "React & TypeScript: The Complete Developer's Guide",
            Subtitle         = "Build modern web applications with React 18, TypeScript, and best practices",
            Description      = "This course takes you from React fundamentals to advanced patterns. You'll master hooks, context, Redux Toolkit, React Query, TypeScript integration, testing with Vitest, and deploying to production. Every concept is backed by real-world projects.",
            ShortDescription = "Master React 18 and TypeScript with real-world projects",
            Language         = "English",
            Level            = CourseLevel.Beginner,
            Status           = CourseStatus.Published,
            Price            = 79.99m,
            DiscountPrice    = 12.99m,
            DiscountExpiry   = DateTime.UtcNow.AddDays(3),
            Currency         = "USD",
            DurationMinutes  = 1200,
            IsFeatured       = true,
            IsLifetimeAccess = true,
            AverageRating    = 4.6,
            TotalReviews     = 892,
            TotalStudents    = 6320,
            TotalLessons     = 98,
            LearningObjectives = "Master React 18 hooks|TypeScript with React|State management with Redux|Testing with Vitest",
            Prerequisites    = "Basic JavaScript knowledge|HTML and CSS fundamentals",
            TargetAudience   = "Developers who want to build modern React applications",
        };

        var course3 = new Course
        {
            Id               = CourseIds.Course3,
            InstructorId     = UserIds.Instructor1,
            Title            = "Microservices Architecture with .NET",
            Subtitle         = "Design and build scalable microservices using .NET, Docker, and Kubernetes",
            Description      = "Learn to design, build and deploy microservices. This course covers service decomposition, API gateways, inter-service communication, Docker containers, Kubernetes orchestration, and monitoring.",
            ShortDescription = "Build production microservices with .NET, Docker and Kubernetes",
            Language         = "English",
            Level            = CourseLevel.Advanced,
            Status           = CourseStatus.Published,
            Price            = 99.99m,
            Currency         = "USD",
            DurationMinutes  = 1800,
            IsFeatured       = false,
            IsLifetimeAccess = true,
            AverageRating    = 4.9,
            TotalReviews     = 423,
            TotalStudents    = 2100,
            TotalLessons     = 145,
            LearningObjectives = "Design microservices|Docker and Kubernetes|API gateways|Event-driven architecture",
            Prerequisites    = "Strong C# and .NET knowledge|Understanding of REST APIs",
            TargetAudience   = "Senior developers ready to scale their applications",
        };

        await context.Courses.AddRangeAsync(course1, course2, course3);
        await context.SaveChangesAsync();

        // Course categories
        await context.CourseCategories.AddRangeAsync(
            new CourseCategory { Id = Guid.NewGuid(), CourseId = CourseIds.Course1, CategoryId = CategoryIds.WebDev,   IsPrimary = true  },
            new CourseCategory { Id = Guid.NewGuid(), CourseId = CourseIds.Course1, CategoryId = CategoryIds.Development, IsPrimary = false },
            new CourseCategory { Id = Guid.NewGuid(), CourseId = CourseIds.Course2, CategoryId = CategoryIds.WebDev,   IsPrimary = true  },
            new CourseCategory { Id = Guid.NewGuid(), CourseId = CourseIds.Course2, CategoryId = CategoryIds.UIDesign, IsPrimary = false },
            new CourseCategory { Id = Guid.NewGuid(), CourseId = CourseIds.Course3, CategoryId = CategoryIds.WebDev,   IsPrimary = true  },
            new CourseCategory { Id = Guid.NewGuid(), CourseId = CourseIds.Course3, CategoryId = CategoryIds.Development, IsPrimary = false }
        );

        // Course tags
        await context.CourseTags.AddRangeAsync(
            new CourseTag { Id = Guid.NewGuid(), CourseId = CourseIds.Course1, TagId = TagIds.CSharp     },
            new CourseTag { Id = Guid.NewGuid(), CourseId = CourseIds.Course1, TagId = TagIds.DotNet     },
            new CourseTag { Id = Guid.NewGuid(), CourseId = CourseIds.Course1, TagId = TagIds.API        },
            new CourseTag { Id = Guid.NewGuid(), CourseId = CourseIds.Course1, TagId = TagIds.CleanCode  },
            new CourseTag { Id = Guid.NewGuid(), CourseId = CourseIds.Course2, TagId = TagIds.React      },
            new CourseTag { Id = Guid.NewGuid(), CourseId = CourseIds.Course2, TagId = TagIds.TypeScript },
            new CourseTag { Id = Guid.NewGuid(), CourseId = CourseIds.Course2, TagId = TagIds.JavaScript },
            new CourseTag { Id = Guid.NewGuid(), CourseId = CourseIds.Course3, TagId = TagIds.CSharp     },
            new CourseTag { Id = Guid.NewGuid(), CourseId = CourseIds.Course3, TagId = TagIds.Docker     },
            new CourseTag { Id = Guid.NewGuid(), CourseId = CourseIds.Course3, TagId = TagIds.Azure      }
        );

        await context.SaveChangesAsync();
        await SeedSectionsAndLessonsAsync(context);
    }

    private static async Task SeedSectionsAndLessonsAsync(ApplicationDbContext context)
    {
        var sections = new List<Section>
        {
            // Course 1 sections
            new() { Id = SectionIds.C1S1, CourseId = CourseIds.Course1, Title = "Getting Started",               DisplayOrder = 1, DurationMinutes = 60  },
            new() { Id = SectionIds.C1S2, CourseId = CourseIds.Course1, Title = "RESTful API Design",            DisplayOrder = 2, DurationMinutes = 180 },
            new() { Id = SectionIds.C1S3, CourseId = CourseIds.Course1, Title = "Clean Architecture",            DisplayOrder = 3, DurationMinutes = 240 },
            new() { Id = SectionIds.C1S4, CourseId = CourseIds.Course1, Title = "Authentication & Authorization",DisplayOrder = 4, DurationMinutes = 150 },
            // Course 2 sections
            new() { Id = SectionIds.C2S1, CourseId = CourseIds.Course2, Title = "React Fundamentals",            DisplayOrder = 1, DurationMinutes = 120 },
            new() { Id = SectionIds.C2S2, CourseId = CourseIds.Course2, Title = "Hooks Deep Dive",               DisplayOrder = 2, DurationMinutes = 180 },
            new() { Id = SectionIds.C2S3, CourseId = CourseIds.Course2, Title = "TypeScript with React",         DisplayOrder = 3, DurationMinutes = 150 },
            // Course 3 sections
            new() { Id = SectionIds.C3S1, CourseId = CourseIds.Course3, Title = "Microservices Fundamentals",    DisplayOrder = 1, DurationMinutes = 120 },
            new() { Id = SectionIds.C3S2, CourseId = CourseIds.Course3, Title = "Docker & Containerization",     DisplayOrder = 2, DurationMinutes = 200 },
        };

        await context.Sections.AddRangeAsync(sections);
        await context.SaveChangesAsync();

        var lessons = new List<Lesson>
        {
            // C1S1 lessons
            new() { Id = LessonIds.C1S1L1, SectionId = SectionIds.C1S1, Title = "Course Introduction",              ContentType = LessonContentType.Video,   DisplayOrder = 1, DurationMinutes = 5,  IsPreview = true  },
            new() { Id = LessonIds.C1S1L2, SectionId = SectionIds.C1S1, Title = "Development Environment Setup",     ContentType = LessonContentType.Video,   DisplayOrder = 2, DurationMinutes = 15, IsPreview = false },
            new() { Id = LessonIds.C1S1L3, SectionId = SectionIds.C1S1, Title = "Project Structure Overview",        ContentType = LessonContentType.Article, DisplayOrder = 3, DurationMinutes = 10, IsPreview = false },
            // C1S2 lessons
            new() { Id = LessonIds.C1S2L1, SectionId = SectionIds.C1S2, Title = "REST Principles & HTTP Methods",    ContentType = LessonContentType.Video,   DisplayOrder = 1, DurationMinutes = 20, IsPreview = true  },
            new() { Id = LessonIds.C1S2L2, SectionId = SectionIds.C1S2, Title = "Controllers & Routing",             ContentType = LessonContentType.Video,   DisplayOrder = 2, DurationMinutes = 25, IsPreview = false },
            new() { Id = LessonIds.C1S2L3, SectionId = SectionIds.C1S2, Title = "Request & Response Models",         ContentType = LessonContentType.Video,   DisplayOrder = 3, DurationMinutes = 20, IsPreview = false },
            new() { Id = LessonIds.C1S2L4, SectionId = SectionIds.C1S2, Title = "Validation with FluentValidation",  ContentType = LessonContentType.Video,   DisplayOrder = 4, DurationMinutes = 30, IsPreview = false },
            // C1S3 lessons
            new() { Id = LessonIds.C1S3L1, SectionId = SectionIds.C1S3, Title = "Clean Architecture Overview",       ContentType = LessonContentType.Video,   DisplayOrder = 1, DurationMinutes = 20, IsPreview = true  },
            new() { Id = LessonIds.C1S3L2, SectionId = SectionIds.C1S3, Title = "Domain Layer Setup",                ContentType = LessonContentType.Video,   DisplayOrder = 2, DurationMinutes = 35, IsPreview = false },
            new() { Id = LessonIds.C1S3L3, SectionId = SectionIds.C1S3, Title = "CQRS with MediatR",                 ContentType = LessonContentType.Video,   DisplayOrder = 3, DurationMinutes = 45, IsPreview = false },
            // C2S1 lessons
            new() { Id = LessonIds.C2S1L1, SectionId = SectionIds.C2S1, Title = "What is React?",                    ContentType = LessonContentType.Video,   DisplayOrder = 1, DurationMinutes = 10, IsPreview = true  },
            new() { Id = LessonIds.C2S1L2, SectionId = SectionIds.C2S1, Title = "JSX and Component Basics",          ContentType = LessonContentType.Video,   DisplayOrder = 2, DurationMinutes = 20, IsPreview = false },
            new() { Id = LessonIds.C2S1L3, SectionId = SectionIds.C2S1, Title = "Props and State",                   ContentType = LessonContentType.Video,   DisplayOrder = 3, DurationMinutes = 25, IsPreview = false },
            // C3S1 lessons
            new() { Id = LessonIds.C3S1L1, SectionId = SectionIds.C3S1, Title = "What Are Microservices?",           ContentType = LessonContentType.Video,   DisplayOrder = 1, DurationMinutes = 15, IsPreview = true  },
            new() { Id = LessonIds.C3S1L2, SectionId = SectionIds.C3S1, Title = "Monolith vs Microservices",         ContentType = LessonContentType.Video,   DisplayOrder = 2, DurationMinutes = 20, IsPreview = false },
            new() { Id = LessonIds.C3S1L3, SectionId = SectionIds.C3S1, Title = "Service Decomposition Strategies",  ContentType = LessonContentType.Article, DisplayOrder = 3, DurationMinutes = 15, IsPreview = false },
        };

        await context.Lessons.AddRangeAsync(lessons);
        await context.SaveChangesAsync();

        // Seed a quiz for a lesson
        var quiz = new Quiz
        {
            Id             = Guid.NewGuid(),
            LessonId       = LessonIds.C1S2L1,
            Title          = "REST Principles Quiz",
            Instructions   = "Answer all questions. You need 70% to pass.",
            PassingScore   = 70,
            MaxAttempts    = 3,
            ShowCorrectAnswers = true,
        };

        await context.Quizzes.AddAsync(quiz);
        await context.SaveChangesAsync();

        var question1 = new Question
        {
            Id = Guid.NewGuid(), QuizId = quiz.Id, Text = "Which HTTP method is used to create a new resource?",
            Explanation = "POST is used to create, PUT to update.", Points = 1, DisplayOrder = 1,
        };
        var question2 = new Question
        {
            Id = Guid.NewGuid(), QuizId = quiz.Id, Text = "What does REST stand for?",
            Explanation = "Representational State Transfer.", Points = 1, DisplayOrder = 2,
        };

        await context.Questions.AddRangeAsync(question1, question2);
        await context.SaveChangesAsync();

        await context.AnswerOptions.AddRangeAsync(
            new AnswerOption { Id = Guid.NewGuid(), QuestionId = question1.Id, Text = "GET",    IsCorrect = false, DisplayOrder = 1 },
            new AnswerOption { Id = Guid.NewGuid(), QuestionId = question1.Id, Text = "POST",   IsCorrect = true,  DisplayOrder = 2 },
            new AnswerOption { Id = Guid.NewGuid(), QuestionId = question1.Id, Text = "DELETE", IsCorrect = false, DisplayOrder = 3 },
            new AnswerOption { Id = Guid.NewGuid(), QuestionId = question1.Id, Text = "PATCH",  IsCorrect = false, DisplayOrder = 4 },
            new AnswerOption { Id = Guid.NewGuid(), QuestionId = question2.Id, Text = "Representational State Transfer", IsCorrect = true,  DisplayOrder = 1 },
            new AnswerOption { Id = Guid.NewGuid(), QuestionId = question2.Id, Text = "Remote Server Technology",        IsCorrect = false, DisplayOrder = 2 },
            new AnswerOption { Id = Guid.NewGuid(), QuestionId = question2.Id, Text = "Relational Schema Transfer",      IsCorrect = false, DisplayOrder = 3 }
        );

        await context.SaveChangesAsync();
    }

    // ─── Enrollments & Progress ─────────────────────────────────────────────

    private static async Task SeedEnrollmentsAndProgressAsync(ApplicationDbContext context)
    {
        if (await context.Enrollments.AnyAsync()) return;

        var enrollments = new List<Enrollment>
        {
            new() { Id = EnrollmentIds.E1, CourseId = CourseIds.Course1, StudentId = UserIds.Student1, Status = EnrollmentStatus.Active,    AmountPaid = 14.99m, EnrolledAt = DateTime.UtcNow.AddDays(-30), ProgressPercentage = 65, LastAccessedAt = DateTime.UtcNow.AddHours(-2) },
            new() { Id = EnrollmentIds.E2, CourseId = CourseIds.Course1, StudentId = UserIds.Student2, Status = EnrollmentStatus.Completed,  AmountPaid = 14.99m, EnrolledAt = DateTime.UtcNow.AddDays(-90), ProgressPercentage = 100, CompletedAt = DateTime.UtcNow.AddDays(-10) },
            new() { Id = EnrollmentIds.E3, CourseId = CourseIds.Course2, StudentId = UserIds.Student1, Status = EnrollmentStatus.Active,    AmountPaid = 12.99m, EnrolledAt = DateTime.UtcNow.AddDays(-15), ProgressPercentage = 30 },
            new() { Id = EnrollmentIds.E4, CourseId = CourseIds.Course2, StudentId = UserIds.Student3, Status = EnrollmentStatus.Active,    AmountPaid = 12.99m, EnrolledAt = DateTime.UtcNow.AddDays(-20), ProgressPercentage = 55 },
            new() { Id = EnrollmentIds.E5, CourseId = CourseIds.Course3, StudentId = UserIds.Student4, Status = EnrollmentStatus.Active,    AmountPaid = 99.99m, EnrolledAt = DateTime.UtcNow.AddDays(-10), ProgressPercentage = 20 },
            new() { Id = EnrollmentIds.E6, CourseId = CourseIds.Course1, StudentId = UserIds.Student5, Status = EnrollmentStatus.Active,    AmountPaid = 14.99m, EnrolledAt = DateTime.UtcNow.AddDays(-5),  ProgressPercentage = 10 },
        };

        await context.Enrollments.AddRangeAsync(enrollments);
        await context.SaveChangesAsync();

        // Progress for enrollment 1 (Student1 in Course1)
        var progresses = new List<Progress>
        {
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E1, LessonId = LessonIds.C1S1L1, IsCompleted = true,  CompletedAt = DateTime.UtcNow.AddDays(-28), TimeSpentMinutes = 5,  VideoWatchedSeconds = 300  },
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E1, LessonId = LessonIds.C1S1L2, IsCompleted = true,  CompletedAt = DateTime.UtcNow.AddDays(-27), TimeSpentMinutes = 18, VideoWatchedSeconds = 900  },
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E1, LessonId = LessonIds.C1S1L3, IsCompleted = true,  CompletedAt = DateTime.UtcNow.AddDays(-26), TimeSpentMinutes = 12 },
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E1, LessonId = LessonIds.C1S2L1, IsCompleted = true,  CompletedAt = DateTime.UtcNow.AddDays(-25), TimeSpentMinutes = 22, VideoWatchedSeconds = 1200 },
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E1, LessonId = LessonIds.C1S2L2, IsCompleted = false, TimeSpentMinutes = 10, VideoWatchedSeconds = 600, VideoTotalSeconds = 1500 },
            // Progress for enrollment 2 (Student2 completed course)
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E2, LessonId = LessonIds.C1S1L1, IsCompleted = true, CompletedAt = DateTime.UtcNow.AddDays(-85) },
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E2, LessonId = LessonIds.C1S1L2, IsCompleted = true, CompletedAt = DateTime.UtcNow.AddDays(-84) },
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E2, LessonId = LessonIds.C1S2L1, IsCompleted = true, CompletedAt = DateTime.UtcNow.AddDays(-80) },
        };

        await context.Progresses.AddRangeAsync(progresses);

        // Payments
        var payments = new List<Payment>
        {
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E1, UserId = UserIds.Student1, TransactionId = "TXN-001", PaymentMethod = "Stripe", Amount = 14.99m, Status = PaymentStatus.Completed, PaidAt = DateTime.UtcNow.AddDays(-30), PlatformFee = 2.25m, InstructorEarnings = 12.74m },
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E2, UserId = UserIds.Student2, TransactionId = "TXN-002", PaymentMethod = "Stripe", Amount = 14.99m, Status = PaymentStatus.Completed, PaidAt = DateTime.UtcNow.AddDays(-90), PlatformFee = 2.25m, InstructorEarnings = 12.74m },
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E3, UserId = UserIds.Student1, TransactionId = "TXN-003", PaymentMethod = "PayPal", Amount = 12.99m, Status = PaymentStatus.Completed, PaidAt = DateTime.UtcNow.AddDays(-15), PlatformFee = 1.95m, InstructorEarnings = 11.04m },
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E4, UserId = UserIds.Student3, TransactionId = "TXN-004", PaymentMethod = "Stripe", Amount = 12.99m, Status = PaymentStatus.Completed, PaidAt = DateTime.UtcNow.AddDays(-20), PlatformFee = 1.95m, InstructorEarnings = 11.04m },
            new() { Id = Guid.NewGuid(), EnrollmentId = EnrollmentIds.E5, UserId = UserIds.Student4, TransactionId = "TXN-005", PaymentMethod = "Stripe", Amount = 99.99m, Status = PaymentStatus.Completed, PaidAt = DateTime.UtcNow.AddDays(-10), PlatformFee = 15.0m, InstructorEarnings = 84.99m },
        };

        await context.Payments.AddRangeAsync(payments);
        await context.SaveChangesAsync();

        // Certificate for completed enrollment
        var certificate = new Certificate
        {
            Id                = Guid.NewGuid(),
            EnrollmentId      = EnrollmentIds.E2,
            CourseId          = CourseIds.Course1,
            StudentId         = UserIds.Student2,
            CertificateNumber = "CERT-2024-0001",
            IssueDate         = DateTime.UtcNow.AddDays(-10),
            VerificationUrl   = "https://elearning.com/verify/CERT-2024-0001",
        };

        await context.Certificates.AddAsync(certificate);
        await context.SaveChangesAsync();
    }

    // ─── Reviews ────────────────────────────────────────────────────────────

    private static async Task SeedReviewsAsync(ApplicationDbContext context)
    {
        if (await context.Reviews.AnyAsync()) return;

        var reviews = new List<Review>
        {
            new()
            {
                Id        = Guid.NewGuid(),
                CourseId  = CourseIds.Course1,
                StudentId = UserIds.Student1,
                Rating    = 5,
                Comment   = "Absolutely incredible course! John explains every concept with clarity and the projects are hands-on. I finally understand Clean Architecture after struggling with it for months.",
                Status    = ReviewStatus.Approved,
                HelpfulCount = 42,
            },
            new()
            {
                Id        = Guid.NewGuid(),
                CourseId  = CourseIds.Course1,
                StudentId = UserIds.Student2,
                Rating    = 5,
                Comment   = "Best .NET course on the platform. The CQRS and MediatR sections are gold. Already applied what I learned at work.",
                Status    = ReviewStatus.Approved,
                HelpfulCount = 31,
            },
            new()
            {
                Id        = Guid.NewGuid(),
                CourseId  = CourseIds.Course2,
                StudentId = UserIds.Student3,
                Rating    = 4,
                Comment   = "Great course for React beginners. TypeScript integration could be more in-depth but overall excellent content.",
                Status    = ReviewStatus.Approved,
                HelpfulCount = 18,
            },
            new()
            {
                Id        = Guid.NewGuid(),
                CourseId  = CourseIds.Course3,
                StudentId = UserIds.Student4,
                Rating    = 5,
                Comment   = "Mind-blowing microservices content. The Docker and Kubernetes sections alone are worth the price.",
                Status    = ReviewStatus.Approved,
                HelpfulCount = 25,
            },
        };

        await context.Reviews.AddRangeAsync(reviews);
        await context.SaveChangesAsync();

        // Announcements
        var announcements = new List<Announcement>
        {
            new()
            {
                Id           = Guid.NewGuid(),
                CourseId     = CourseIds.Course1,
                InstructorId = UserIds.Instructor1,
                Title        = "New Section Added: Advanced Caching Strategies",
                Content      = "I've just uploaded 5 new lessons covering Redis caching, distributed caching patterns, and cache invalidation strategies. Check them out in Section 8!",
                IsPinned     = true,
            },
            new()
            {
                Id           = Guid.NewGuid(),
                CourseId     = CourseIds.Course2,
                InstructorId = UserIds.Instructor2,
                Title        = "Course Updated for React 18",
                Content      = "All lessons have been updated to cover React 18 changes including the new concurrent features and automatic batching.",
                IsPinned     = false,
            },
        };

        await context.Announcements.AddRangeAsync(announcements);
        await context.SaveChangesAsync();

        // Discussions
        var discussion = new Discussion
        {
            Id           = Guid.NewGuid(),
            CourseId     = CourseIds.Course1,
            LessonId     = LessonIds.C1S3L3,
            UserId       = UserIds.Student1,
            Type         = DiscussionType.Question,
            Title        = "When should I use Commands vs Queries in CQRS?",
            Content      = "I understand Commands change state and Queries read state, but I'm confused about complex read operations that might trigger side effects. Should those be Commands?",
            UpvoteCount  = 12,
            ReplyCount   = 1,
        };

        await context.Discussions.AddAsync(discussion);
        await context.SaveChangesAsync();

        var reply = new DiscussionReply
        {
            Id              = Guid.NewGuid(),
            DiscussionId    = discussion.Id,
            UserId          = UserIds.Instructor1,
            Content         = "Great question! The rule of thumb is: if it changes state → Command; if it only reads → Query. For side-effect-triggering reads, create a Command. Auditing a read (like 'user viewed this') is a separate Command triggered after the Query.",
            IsInstructorAnswer = true,
            UpvoteCount     = 8,
        };

        await context.DiscussionReplies.AddAsync(reply);
        await context.SaveChangesAsync();
    }

    // ─── Coupons ────────────────────────────────────────────────────────────

    private static async Task SeedCouponsAsync(ApplicationDbContext context)
    {
        if (await context.Coupons.AnyAsync()) return;

        var coupons = new List<Coupon>
        {
            new()
            {
                Id            = Guid.NewGuid(),
                Code          = "WELCOME50",
                Description   = "50% off for new students",
                Type          = CouponType.Percentage,
                Value         = 50,
                MaxDiscountAmount = 20,
                MinOrderAmount    = 10,
                ExpiryDate    = DateTime.UtcNow.AddMonths(3),
                MaxUses       = 1000,
                CurrentUses   = 243,
                MaxUsesPerUser = 1,
                IsActive      = true,
            },
            new()
            {
                Id            = Guid.NewGuid(),
                Code          = "SAVE10",
                Description   = "$10 off any course",
                Type          = CouponType.FixedAmount,
                Value         = 10,
                MinOrderAmount = 15,
                ExpiryDate    = DateTime.UtcNow.AddMonths(1),
                MaxUses       = 500,
                CurrentUses   = 89,
                MaxUsesPerUser = 1,
                IsActive      = true,
            },
            new()
            {
                Id            = Guid.NewGuid(),
                Code          = "FLASH24",
                Description   = "24-hour flash sale — 70% off",
                Type          = CouponType.Percentage,
                Value         = 70,
                MaxDiscountAmount = 30,
                ExpiryDate    = DateTime.UtcNow.AddDays(1),
                MaxUses       = 200,
                CurrentUses   = 156,
                MaxUsesPerUser = 1,
                IsActive      = true,
            },
        };

        await context.Coupons.AddRangeAsync(coupons);
        await context.SaveChangesAsync();
    }

    // ─── Notifications ──────────────────────────────────────────────────────

    private static async Task SeedNotificationsAsync(ApplicationDbContext context)
    {
        if (await context.Notifications.AnyAsync()) return;

        var notifications = new List<Notification>
        {
            new()
            {
                Id          = Guid.NewGuid(),
                RecipientId = UserIds.Student1,
                Title       = "New lesson available!",
                Message     = "A new lesson 'Advanced Caching Strategies' has been added to your enrolled course.",
                Type        = NotificationType.NewContent,
                Channel     = NotificationChannel.InApp,
                Status      = NotificationStatus.Unread,
            },
            new()
            {
                Id          = Guid.NewGuid(),
                RecipientId = UserIds.Student2,
                Title       = "Certificate Ready!",
                Message     = "Your certificate for 'Complete ASP.NET Core Web API Development' is ready to download.",
                Type        = NotificationType.CourseCompleted,
                Channel     = NotificationChannel.InApp,
                Status      = NotificationStatus.Read,
                ReadAt      = DateTime.UtcNow.AddDays(-9),
            },
            new()
            {
                Id          = Guid.NewGuid(),
                RecipientId = UserIds.Instructor1,
                Title       = "New review received",
                Message     = "Alice Brown left a 5-star review on your course.",
                Type        = NotificationType.NewReview,
                Channel     = NotificationChannel.InApp,
                Status      = NotificationStatus.Read,
                ReadAt      = DateTime.UtcNow.AddDays(-1),
            },
            new()
            {
                Id          = Guid.NewGuid(),
                RecipientId = UserIds.Student3,
                Title       = "Flash Sale! 70% off today only",
                Message     = "Use code FLASH24 to get 70% off any course. Offer expires in 24 hours!",
                Type        = NotificationType.Promotion,
                Channel     = NotificationChannel.Email,
                Status      = NotificationStatus.Unread,
            },
        };

        await context.Notifications.AddRangeAsync(notifications);

        // Wishlists
        await context.Wishlists.AddRangeAsync(
            new Wishlist { Id = Guid.NewGuid(), UserId = UserIds.Student1, CourseId = CourseIds.Course3, AddedAt = DateTime.UtcNow.AddDays(-5)  },
            new Wishlist { Id = Guid.NewGuid(), UserId = UserIds.Student3, CourseId = CourseIds.Course1, AddedAt = DateTime.UtcNow.AddDays(-12) },
            new Wishlist { Id = Guid.NewGuid(), UserId = UserIds.Student5, CourseId = CourseIds.Course2, AddedAt = DateTime.UtcNow.AddDays(-3)  }
        );

        await context.SaveChangesAsync();
    }

    // ─── Helpers ────────────────────────────────────────────────────────────

    private static string BCryptHash(string password) =>
        BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);
}

// ─── Seed ID Constants ───────────────────────────────────────────────────────

public static class RoleIds
{
    public static readonly Guid Admin      = Guid.Parse("11111111-0000-0000-0000-000000000001");
    public static readonly Guid Instructor = Guid.Parse("11111111-0000-0000-0000-000000000002");
    public static readonly Guid Student    = Guid.Parse("11111111-0000-0000-0000-000000000003");
}

public static class UserIds
{
    public static readonly Guid Admin       = Guid.Parse("22222222-0000-0000-0000-000000000001");
    public static readonly Guid Instructor1 = Guid.Parse("22222222-0000-0000-0000-000000000002");
    public static readonly Guid Instructor2 = Guid.Parse("22222222-0000-0000-0000-000000000003");
    public static readonly Guid Student1    = Guid.Parse("22222222-0000-0000-0000-000000000004");
    public static readonly Guid Student2    = Guid.Parse("22222222-0000-0000-0000-000000000005");
    public static readonly Guid Student3    = Guid.Parse("22222222-0000-0000-0000-000000000006");
    public static readonly Guid Student4    = Guid.Parse("22222222-0000-0000-0000-000000000007");
    public static readonly Guid Student5    = Guid.Parse("22222222-0000-0000-0000-000000000008");
}

public static class CategoryIds
{
    public static readonly Guid Development = Guid.Parse("33333333-0000-0000-0000-000000000001");
    public static readonly Guid Design      = Guid.Parse("33333333-0000-0000-0000-000000000002");
    public static readonly Guid Business    = Guid.Parse("33333333-0000-0000-0000-000000000003");
    public static readonly Guid Marketing   = Guid.Parse("33333333-0000-0000-0000-000000000004");
    public static readonly Guid WebDev      = Guid.Parse("33333333-0000-0000-0000-000000000005");
    public static readonly Guid MobileDev   = Guid.Parse("33333333-0000-0000-0000-000000000006");
    public static readonly Guid DataSci     = Guid.Parse("33333333-0000-0000-0000-000000000007");
    public static readonly Guid UIDesign    = Guid.Parse("33333333-0000-0000-0000-000000000008");
    public static readonly Guid GraphicDes  = Guid.Parse("33333333-0000-0000-0000-000000000009");
}

public static class TagIds
{
    public static readonly Guid CSharp     = Guid.Parse("44444444-0000-0000-0000-000000000001");
    public static readonly Guid DotNet     = Guid.Parse("44444444-0000-0000-0000-000000000002");
    public static readonly Guid React      = Guid.Parse("44444444-0000-0000-0000-000000000003");
    public static readonly Guid JavaScript = Guid.Parse("44444444-0000-0000-0000-000000000004");
    public static readonly Guid TypeScript = Guid.Parse("44444444-0000-0000-0000-000000000005");
    public static readonly Guid Python     = Guid.Parse("44444444-0000-0000-0000-000000000006");
    public static readonly Guid Docker     = Guid.Parse("44444444-0000-0000-0000-000000000007");
    public static readonly Guid Azure      = Guid.Parse("44444444-0000-0000-0000-000000000008");
    public static readonly Guid CleanCode  = Guid.Parse("44444444-0000-0000-0000-000000000009");
    public static readonly Guid API        = Guid.Parse("44444444-0000-0000-0000-000000000010");
}

public static class CourseIds
{
    public static readonly Guid Course1 = Guid.Parse("55555555-0000-0000-0000-000000000001");
    public static readonly Guid Course2 = Guid.Parse("55555555-0000-0000-0000-000000000002");
    public static readonly Guid Course3 = Guid.Parse("55555555-0000-0000-0000-000000000003");
}

public static class SectionIds
{
    public static readonly Guid C1S1 = Guid.Parse("66666666-0000-0000-0000-000000000001");
    public static readonly Guid C1S2 = Guid.Parse("66666666-0000-0000-0000-000000000002");
    public static readonly Guid C1S3 = Guid.Parse("66666666-0000-0000-0000-000000000003");
    public static readonly Guid C1S4 = Guid.Parse("66666666-0000-0000-0000-000000000004");
    public static readonly Guid C2S1 = Guid.Parse("66666666-0000-0000-0000-000000000005");
    public static readonly Guid C2S2 = Guid.Parse("66666666-0000-0000-0000-000000000006");
    public static readonly Guid C2S3 = Guid.Parse("66666666-0000-0000-0000-000000000007");
    public static readonly Guid C3S1 = Guid.Parse("66666666-0000-0000-0000-000000000008");
    public static readonly Guid C3S2 = Guid.Parse("66666666-0000-0000-0000-000000000009");
}

public static class LessonIds
{
    public static readonly Guid C1S1L1 = Guid.Parse("77777777-0000-0000-0000-000000000001");
    public static readonly Guid C1S1L2 = Guid.Parse("77777777-0000-0000-0000-000000000002");
    public static readonly Guid C1S1L3 = Guid.Parse("77777777-0000-0000-0000-000000000003");
    public static readonly Guid C1S2L1 = Guid.Parse("77777777-0000-0000-0000-000000000004");
    public static readonly Guid C1S2L2 = Guid.Parse("77777777-0000-0000-0000-000000000005");
    public static readonly Guid C1S2L3 = Guid.Parse("77777777-0000-0000-0000-000000000006");
    public static readonly Guid C1S2L4 = Guid.Parse("77777777-0000-0000-0000-000000000007");
    public static readonly Guid C1S3L1 = Guid.Parse("77777777-0000-0000-0000-000000000008");
    public static readonly Guid C1S3L2 = Guid.Parse("77777777-0000-0000-0000-000000000009");
    public static readonly Guid C1S3L3 = Guid.Parse("77777777-0000-0000-0000-000000000010");
    public static readonly Guid C2S1L1 = Guid.Parse("77777777-0000-0000-0000-000000000011");
    public static readonly Guid C2S1L2 = Guid.Parse("77777777-0000-0000-0000-000000000012");
    public static readonly Guid C2S1L3 = Guid.Parse("77777777-0000-0000-0000-000000000013");
    public static readonly Guid C3S1L1 = Guid.Parse("77777777-0000-0000-0000-000000000014");
    public static readonly Guid C3S1L2 = Guid.Parse("77777777-0000-0000-0000-000000000015");
    public static readonly Guid C3S1L3 = Guid.Parse("77777777-0000-0000-0000-000000000016");
}

public static class EnrollmentIds
{
    public static readonly Guid E1 = Guid.Parse("88888888-0000-0000-0000-000000000001");
    public static readonly Guid E2 = Guid.Parse("88888888-0000-0000-0000-000000000002");
    public static readonly Guid E3 = Guid.Parse("88888888-0000-0000-0000-000000000003");
    public static readonly Guid E4 = Guid.Parse("88888888-0000-0000-0000-000000000004");
    public static readonly Guid E5 = Guid.Parse("88888888-0000-0000-0000-000000000005");
    public static readonly Guid E6 = Guid.Parse("88888888-0000-0000-0000-000000000006");
}
