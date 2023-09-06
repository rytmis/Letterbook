using Microsoft.AspNetCore.Identity;

namespace Letterbook.Core.Models;

public class AccountAuthentication : IdentityUser<Guid>
{
    public Account Account { get; set; }

    private AccountAuthentication()
    {
        Account = default!;
    }

    public AccountAuthentication(Account account, string email, string? username)
    {
        Account = account;
        base.Email = email;
        base.EmailConfirmed = false;
        base.UserName = username;
    }
}