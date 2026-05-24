using System;

namespace Domain.Opinions;

public class Opinion(string title, string desc, string userId, int points)
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
    public string UserId { get; private set; } = userId;
    public int Points { get; private set; } = points;
    public string Title { get; private set; } = title;
    public string Desc { get; private set; } = desc;
}