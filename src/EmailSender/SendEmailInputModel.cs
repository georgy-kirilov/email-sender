namespace EmailSender;

public sealed record SendEmailInputModel(
    string EmailOfReceiver,
    string Subject,
    string Body,
    IEnumerable<IFormFile>? Attachments);
