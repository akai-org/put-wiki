using System;

namespace Domain.Users;

public class User
{
    public required Guid Id { get; init; }
    public required string HashedUsosId { get; init; }
    public required DateTimeOffset JoinedDate { get; init; }

    private User() { }

    public static User Create(string hashedUsosId, DateTimeOffset joinedDate)
    {
        if (string.IsNullOrWhiteSpace(hashedUsosId))
            throw new ArgumentException("Hashed USOS ID cannot be empty.");

        var user = new User()
        {
            Id = Guid.CreateVersion7(),
            HashedUsosId = hashedUsosId,
            JoinedDate = joinedDate
        };

        return user;
    }
}