namespace EmailSender;

public interface IEmailService
{
    Task SendEmail(SendEmailInputModel input);
}
