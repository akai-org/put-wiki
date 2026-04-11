namespace Domain;

public class User
{
    public string Id { get; private set; }
    public string Nickname { get; private set; }
    public int Karma { get; private set; }
    public DateOnly JoinDate { get; private set; }

    public User(string nickname)
    {
        Id = Guid.NewGuid().ToString();
        Nickname = nickname;
        Karma = 0;
        JoinDate = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public void AddKarma(int amount) => Karma += amount;
}