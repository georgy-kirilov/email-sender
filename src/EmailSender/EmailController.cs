using Microsoft.AspNetCore.Mvc;

namespace EmailSender;

[ApiController]
public sealed class EmailController : ControllerBase
{
    [HttpPost("send/mailkit")]
    public async Task<IActionResult> SendEmailUsingMailKit(
        [FromForm] SendEmailInputModel input,
        [FromServices] MailKitEmailService service)
    {
        await service.SendEmail(input);
        return Ok();
    }

    [HttpPost("send/system-net-mail")]
    public IActionResult SendEmailUsingSystemNetMail(
        bool isBodyHtml,
        [FromForm] SendEmailInputModel input,
        [FromServices] SystemNetMailEmailService service)
    {
        service.SendEmail(input, isBodyHtml);
        return Ok();
    }
}
