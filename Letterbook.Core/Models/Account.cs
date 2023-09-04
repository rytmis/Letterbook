﻿using Letterbook.Core.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Letterbook.Core.Models;

public class Account
{
    public string Id { get; set; }
    public string Email { get; set; }
    public IdentityUser AccountIdentity { get; set; }
    public ICollection<LinkedProfile> LinkedProfiles { get; set; } = new HashSet<LinkedProfile>();
    
    // In the future, Account might tie in to things like organizations or billing accounts
    
    private Account()
    {
        Id = default!;
        Email = default!;
    }

    private Account(string email) : this()
    {
        Id = ShortId.NewShortId();
        Email = email;
    }

    // TODO(Account creation): https://github.com/Letterbook/Letterbook/issues/32
    public static Account CreateAccount(Uri baseUri, string email, string handle)
    {
        var profile = Profile.CreatePerson(baseUri, handle);
        var account = new Account(email);
        profile.OwnedBy = account;
        account.LinkedProfiles.Add(new LinkedProfile(account, profile, ProfilePermission.All));

        return account;
    }
}