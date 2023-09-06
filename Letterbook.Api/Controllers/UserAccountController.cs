using System.Security.Claims;
using Letterbook.Api.Dto;
using Letterbook.Core;
using Letterbook.Core.Exceptions;
using Letterbook.Core.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation.AspNetCore;

namespace Letterbook.Api.Controllers;

[Route("/api/v1/[controller]/[action]")]
public class UserAccountController : ControllerBase
{
    private readonly ILogger<UserAccountController> _logger;
    private readonly IAccountService _accountService;

    public UserAccountController(ILogger<UserAccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody]LoginRequest loginRequest)
    {
        try
        {
            var identity = await _accountService.AuthenticatePassword(loginRequest.Email, loginRequest.Password);
            if (identity == null) return Unauthorized();

            var principal = new ClaimsPrincipal(identity);
            // Scheme? Need a scheme. What scheme is this?
            return SignIn(principal);
        }
        catch (RateLimitException e)
        {
            return StatusCode(429, new { e.Expiration, e.Message });
        }
    }
    
    [HttpPost]
    public IActionResult Logout()
    {
        var controller = nameof(Logout);
        _logger.LogInformation("{Controller}", controller);
     
        throw new NotImplementedException();
    }
    
    [HttpPost]
    public async Task<IActionResult> Register([FromBody]RegistrationRequest registrationRequest)
    {
        var controller = nameof(Register);
        _logger.LogInformation("{Controller}", controller);

        try
        {
            var account = _accountService.RegisterAccount(registrationRequest.Email, registrationRequest.Handle);
            
            // account created, now what?
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

        throw new NotImplementedException();
    }
}