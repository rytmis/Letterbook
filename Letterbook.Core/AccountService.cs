using System.Security.Claims;
using Letterbook.Core.Adapters;
using Letterbook.Core.Exceptions;
using Letterbook.Core.Extensions;
using Letterbook.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Letterbook.Core;

public class AccountService : IAccountService, IDisposable
{
    private readonly ILogger<AccountService> _logger;
    private readonly CoreOptions _opts;
    private readonly IAccountProfileAdapter _accountAdapter;
    private readonly IAccountEventService _eventService;
    private readonly UserManager<AccountAuthentication> _userManager;

    public AccountService(ILogger<AccountService> logger, IOptions<CoreOptions> options,
        IAccountProfileAdapter accountAdapter, IAccountEventService eventService,
        UserManager<AccountAuthentication> userManager)
    {
        _logger = logger;
        _opts = options.Value;
        _accountAdapter = accountAdapter;
        _eventService = eventService;
        _userManager = userManager;
    }

    public async Task<ClaimsIdentity?> AuthenticatePassword(string email, string password)
    {
        var accountAuth = await _userManager.FindByEmailAsync(email);
        if (accountAuth == null) return null;
        if (accountAuth.LockoutEnd >= DateTime.UtcNow)
        {
            throw new RateLimitException("Too many failed attempts", accountAuth.LockoutEnd.GetValueOrDefault());
        }

        var match = _userManager.PasswordHasher.VerifyHashedPassword(accountAuth,
            accountAuth.PasswordHash ?? string.Empty, password);
        switch (match)
        {
            case PasswordVerificationResult.SuccessRehashNeeded:
                await _userManager.ResetAccessFailedCountAsync(accountAuth);
                accountAuth.PasswordHash = _userManager.PasswordHasher.HashPassword(accountAuth, password);
                return new ClaimsIdentity(await _userManager.GetClaimsAsync(accountAuth));
            case PasswordVerificationResult.Success:
                await _userManager.ResetAccessFailedCountAsync(accountAuth);
                return new ClaimsIdentity(await _userManager.GetClaimsAsync(accountAuth));
            case PasswordVerificationResult.Failed:
            default:
                await _userManager.AccessFailedAsync(accountAuth);
                _logger.LogInformation("Password Authentication failed for {AccountId}", accountAuth.Account.Id);
                return default;
        }
    }

    public Account? RegisterAccount(string email, string handle)
    {
        var baseUri = _opts.BaseUri();
        var account = Account.CreateAccount(baseUri, email, handle);

        var success = _accountAdapter.RecordAccount(account);
        if (success)
        {
            _logger.LogInformation("Created new account {AccountId}", account.Id);
            _eventService.Created(account);
            return account;
        }

        _logger.LogWarning("Could not create new account for {Email}", account.Email);
        return default;
    }

    public Account? LookupAccount(string id)
    {
        return _accountAdapter.LookupAccount(id);
    }

    public Profile LookupProfile(string queryTarget)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Account> FindAccounts(string email)
    {
        throw new NotImplementedException();
    }

    public bool UpdateEmail(string accountId, string email)
    {
        var account = _accountAdapter.LookupAccount(accountId);
        if (account == null) return false;
        account.Email = email;
        return true;
    }

    public bool AddLinkedProfile(string accountId, Profile profile, ProfilePermission permission)
    {
        var account = _accountAdapter.LookupAccount(accountId);
        if (account is null) return false;
        var count = account.LinkedProfiles.Count;
        var link = new LinkedProfile(account, profile, permission);
        profile.RelatedAccounts.Add(link);
        account.LinkedProfiles.Add(link);
        return count == account.LinkedProfiles.Count;
    }

    public bool UpdateLinkedProfile(string accountId, Profile profile, ProfilePermission permission)
    {
        var account = _accountAdapter.LookupAccount(accountId);
        if (account is null) return false;
        var model = new LinkedProfile(account, profile, ProfilePermission.None);
        var profileLink = profile.RelatedAccounts.SingleOrDefault(p => p.Equals(model));
        var accountLink = account.LinkedProfiles.SingleOrDefault(p => p.Equals(model));
        if (profileLink is null || accountLink is null)
        {
            _logger.LogWarning("Account {Account} and Profile {Profile} have mismatched permission links", account.Id,
                profile.Id);
            return false;
        }

        if (profileLink.Permission == permission || accountLink.Permission == permission) return false;

        profileLink.Permission = permission;
        accountLink.Permission = permission;
        return true;
    }

    public bool RemoveLinkedProfile(string accountId, Profile profile)
    {
        var account = _accountAdapter.LookupAccount(accountId);
        if (account is null) return false;

        var link = new LinkedProfile(account, profile, ProfilePermission.None);

        return profile.RelatedAccounts.Remove(link) && account.LinkedProfiles.Remove(link);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _userManager.Dispose();
    }
}