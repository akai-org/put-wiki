using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser
{
    public string Nickname { get; set; }
    public int Karma { get; set; }
    public DateOnly JoinDate { get; set; }

    // we could use Opinion instead of string but that would couple domain model to EF Core's
    // idk if we really need it - we are storing UserId in a Opinion but imo that's better for easier search
    public List<string> OpinionIds { get; set; } = new();

    public User(string nickname)
    {
        Id = Guid.NewGuid().ToString();
        Nickname = nickname;
        Karma = 0;
        JoinDate = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public void AddKarma(int amount) => Karma += amount;
}
