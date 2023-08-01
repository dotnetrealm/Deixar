using Deixar.API.Commons;
using Deixar.Domain.DTOs;
using Deixar.Domain.Interfaces;
using Deixar.Domain.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Deixar.API.Controllers.V2;

[LogMethod]
[ApiController]
[ApiVersion("2.0")]
[EndpointGroupName("v2")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly TokenUtility _tokenUtility;

    public AccountController(IAccountRepository accountRepository, TokenUtility tokenUtility)
    {
        _accountRepository = accountRepository;
        _tokenUtility = tokenUtility;
    }

    /// <summary>
    /// Check given credentials and if it is valid than return token
    /// </summary>
    /// <param name="creds"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> LoginAsync([FromBody] Credentials creds)
    {
        if (!await _accountRepository.IsUserExist(creds.EmailAddress)) return BadRequest(new { Error = "User not found!" });
        UserDetails? user = await _accountRepository.GetUserByEmailPasswordAsync(creds.EmailAddress, creds.Password);
        string token = _tokenUtility.GenerateJWT(user);
        return Ok(new { Success = "Login successful.", Token = token });
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUserAsync([FromBody] RegisterUserModel user)
    {
        if (!ModelState.IsValid) return BadRequest(new { Error = "Users detail not valid" });
        int id = await _accountRepository.RegisterUserAsync(user);
        return Ok(new { Success = "Login successful." });
    }
}
