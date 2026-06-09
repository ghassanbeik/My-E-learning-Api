namespace Horizon.Domain.Enums;

public enum CourseLevel
{
    AllLevels    = 0,
    Beginner     = 1,
    Intermediate = 2,
    Advanced     = 3,
}

public enum CourseStatus
{
    Draft      = 0,
    UnderReview = 1,
    Published  = 2,
    Archived   = 3,
    Rejected   = 4,
}

public enum LessonContentType
{
    Video    = 0,
    Article  = 1,
    Resource = 2,
    Quiz     = 3,
}

public enum EnrollmentStatus
{
    Active    = 0,
    Completed = 1,
    Expired   = 2,
    Refunded  = 3,
    Suspended = 4,
}

public enum ReviewStatus
{
    Pending  = 0,
    Approved = 1,
    Rejected = 2,
    Flagged  = 3,
}

public enum PaymentStatus
{
    Pending   = 0,
    Completed = 1,
    Failed    = 2,
    Refunded  = 3,
    Cancelled = 4,
    Disputed  = 5,
}

public enum RefundStatus
{
    Pending  = 0,
    Approved = 1,
    Rejected = 2,
    Processed = 3,
}

public enum PayoutStatus
{
    Pending    = 0,
    Processing = 1,
    Completed  = 2,
    Failed     = 3,
    Cancelled  = 4,
}

public enum CouponType
{
    Percentage  = 0,
    FixedAmount = 1,
}

public enum NotificationType
{
    SystemAnnouncement = 0,
    NewEnrollment      = 1,
    CourseCompleted    = 2,
    NewReview          = 3,
    NewContent         = 4,
    PaymentReceived    = 5,
    PayoutProcessed    = 6,
    Promotion          = 7,
    NewDiscussion      = 8,
    DiscussionReply    = 9,
    AssignmentGraded   = 10,
    QuizResult         = 11,
    CertificateIssued  = 12,
    LiveSessionStarting = 13,
    CourseApproved     = 14,
    CourseRejected     = 15,
}

public enum NotificationChannel
{
    InApp = 0,
    Email = 1,
    Push  = 2,
    Sms   = 3,
}

public enum NotificationStatus
{
    Unread    = 0,
    Read      = 1,
    Archived  = 2,
    Failed    = 3,
}

public enum DiscussionType
{
    General  = 0,
    Question = 1,
    Feedback = 2,
    Bug      = 3,
}
