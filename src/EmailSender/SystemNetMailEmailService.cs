using System.Net;
using System.Net.Mail;

using Microsoft.Extensions.Options;

namespace EmailSender;

public sealed class SystemNetMailEmailService
{
    private readonly EmailOptions _emailOptions;

    public SystemNetMailEmailService(IOptions<EmailOptions> options)
    {
        _emailOptions = options.Value;
    }

    public void SendEmail(SendEmailInputModel input, bool isBodyHtml = false)
    {
        var to = new MailAddress(input.EmailOfReceiver);

        var from = new MailAddress(_emailOptions.EmailOfSender);

        var message = new MailMessage(from, to)
        {
            Subject = input.Subject,
            Body = input.Body,
            IsBodyHtml = isBodyHtml,
        };

        AddAttachmentsTo(message, input.Attachments);

        var smtp = new SmtpClient(_emailOptions.Host, _emailOptions.Port)
        {
            Credentials = new NetworkCredential(_emailOptions.EmailOfSender, _emailOptions.Password),
            EnableSsl = true
        };

        smtp.Send(message);
    }

    private static void AddAttachmentsTo(MailMessage message, IEnumerable<IFormFile>? attachments)
    {
        if (attachments is null || !attachments.Any())
        {
            return;
        }

        foreach (var attachment in attachments)
        {
            if (attachment.Length <= 0)
            {
                continue;
            }

            var attachmentToAdd = new Attachment(attachment.OpenReadStream(), attachment.FileName);

            message.Attachments.Add(attachmentToAdd);
        }
    }
}
