using System;

namespace Domain.Users;

public class User
{
    public Guid Id { get; private set; }
    public string HashedUsosId { get; private set; }

    public User(string hashedUsosId)
    {
        if (string.IsNullOrWhiteSpace(hashedUsosId))
            throw new ArgumentException("Hashed USOS ID cannot be empty.");

        Id = Guid.CreateVersion7();
        HashedUsosId = hashedUsosId;
    }
}