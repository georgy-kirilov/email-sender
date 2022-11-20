using Microsoft.AspNetCore.Mvc;

namespace EmailSender;

[ApiController]
[Route("[controller]")]
public sealed class EmailsController : ControllerBase
{
    private readonly IEmailService _emailService;

    public EmailsController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost("send/mailkit")]
    public async Task<IActionResult> Send([FromForm] SendEmailInputModel input)
    {
        await _emailService.SendEmail(input);
        return Ok();
    }
}
