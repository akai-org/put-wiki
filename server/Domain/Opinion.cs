using System;

namespace Domain;

public class Opinion
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public int Points { get; set; }
    public string Title { get; set; }
    public string Desc { get; set; }

    public Opinion(string title, string desc, string userId)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        Points = 0;
        Title = title;
        Desc = desc;
    }
}