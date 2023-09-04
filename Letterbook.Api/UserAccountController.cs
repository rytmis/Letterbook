using Letterbook.Core;
using Microsoft.AspNetCore.Mvc;

namespace Letterbook.Api;

[Route("api/v1/[controller]")]
public class UserAccountController
{
    private readonly ILogger<UserAccountController> _logger;
    private readonly IAccountService _accountService;

    // private readonly IMessageService _messageService;

    public UserAccountController(ILogger<UserAccountController> logger, IAccountService accountService)
    {
        _logger = logger;
        _accountService = accountService;
    }
}