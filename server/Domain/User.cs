using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser
{
    public string Id { get; set; }
    public string Nickname { get; set; }
    public int Karma { get; set; }
    public DateOnly JoinDate { get; set; }
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
