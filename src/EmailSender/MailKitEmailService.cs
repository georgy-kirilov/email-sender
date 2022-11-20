using Microsoft.Extensions.Options;

using MailKit.Net.Smtp;
using MailKit.Security;

using MimeKit;

namespace EmailSender;

public sealed class MailKitEmailService
{
    private readonly EmailOptions _emailOptions;

    public MailKitEmailService(IOptions<EmailOptions> options)
    {
        _emailOptions = options.Value;
    }

    public async Task SendEmail(SendEmailInputModel input)
    {
        var email = await CreateEmail(input);

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(_emailOptions.EmailOfSender, _emailOptions.Password);

        await smtp.SendAsync(email);

        await smtp.DisconnectAsync(true);
    }

    private async Task<MimeMessage> CreateEmail(SendEmailInputModel input)
    {
        var email = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_emailOptions.EmailOfSender),
            Subject = input.Subject,
        };

        email.To.Add(MailboxAddress.Parse(input.EmailOfReceiver));

        var builder = new BodyBuilder();

        await AddAttachmentsTo(builder, input.Attachments);

        builder.HtmlBody = input.Body;

        email.Body = builder.ToMessageBody();

        return email;
    }

    private static async Task AddAttachmentsTo(BodyBuilder builder, IEnumerable<IFormFile>? attachments)
    {
        if (attachments is null || !attachments.Any())
        {
            return;
        }

        byte[] fileBytes;

        foreach (var attachment in attachments)
        {
            if (attachment.Length <= 0)
            {
                continue;
            }

            using var stream = new MemoryStream();

            await attachment.CopyToAsync(stream);

            fileBytes = stream.ToArray();

            var contentType = ContentType.Parse(attachment.ContentType);

            builder.Attachments.Add(attachment.FileName, fileBytes, contentType);
        }
    }
}
