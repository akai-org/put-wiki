using System;
using System.Collections.Generic;

namespace Domain;

/*
1. We should use IdentityUser here to the User but we don't want to have pure models
We will map our domain User directly to the Identity tables by configuring EF Core to treat it as the Identity user, or
we can create a separate ApplicationUser : IdentityUser in Infrastructure and map between the two.

2. I added ApplicationUser to the Infra/Identity/ApplicationUser.cs where we are using IdentityUser. 
WE STILL NEED TO DO MAPPING 
*/
public class User
{
    public string Id { get; set; }
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