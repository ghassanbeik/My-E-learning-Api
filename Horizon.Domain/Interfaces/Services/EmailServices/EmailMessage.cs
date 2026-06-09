namespace Horizon.Domain.Interfaces.Services.EmailServices
{
    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string HtmlBody { get; set; } = string.Empty;
        public string? PlainTextBody { get; set; }
        public string? ReplyTo { get; set; }
        public List<EmailAttachment> Attachments { get; set; } = new();
    }
}
