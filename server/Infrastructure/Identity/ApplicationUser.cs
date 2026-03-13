using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string Nickname { get; set; } = string.Empty;
    public int Karma { get; set; }
    public DateOnly JoinDate { get; set; }
    public List<string> OpinionIds { get; set; } = new();
}
