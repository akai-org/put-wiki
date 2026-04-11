using System;

namespace Domain;

public class Opinion
{
    public string Id { get; private set; }
    public string UserId { get; private set; }
    public int Points { get; private set; }
    public string Title { get; private set; }
    public string Desc { get; private set; }

    public Opinion(string title, string desc, string userId)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        Points = 0;
        Title = title;
        Desc = desc;
    }
}