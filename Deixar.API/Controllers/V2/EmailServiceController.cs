using Deixar.API.Commons;
using Deixar.Domain.Interfaces;
using Deixar.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deixar.API.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[EndpointGroupName("v2")]
[LogMethod]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EmailServiceController : ControllerBase
{
    private readonly IEmailService _emailService;
    public EmailServiceController(IEmailService emailService)
    {
        _emailService = emailService;
    }

    /// <summary>
    /// Send mail
    /// </summary>
    [Authorize(Roles = "HR")]
    [HttpPost]
    public void SendMail(string[] to, string subject, string content)
    {
        var msg = new Message(to, subject, content);
        _emailService.SendMail(msg);
    }
}
