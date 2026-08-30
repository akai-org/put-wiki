using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Errors;
using Application.Features.Users.Commands.UpdateNickname;

using Domain.Users;

using FluentAssertions;

using FluentResults;

using Microsoft.Extensions.Logging.Abstractions;

using Moq;

namespace PutWiki.UnitTests.Application;

public class UpdateNicknameUseCaseTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UpdateNicknameUseCase _sut;

    public UpdateNicknameUseCaseTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();

        _sut = new UpdateNicknameUseCase(
            _userRepositoryMock.Object,
            NullLogger<UpdateNicknameUseCase>.Instance
        );
    }

    [Fact]
    public async Task ExecuteAsync_WithValidAndUniqueNickname_ShouldSucceed()
    {
        // Arrange
        var user = new User("hashed_id", DateTimeOffset.UtcNow);
        var cmd = new UpdateNicknameCommand(user.Id, "ValidNick");

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.ExistsWithNicknameAsync("ValidNick", user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        user.Nickname!.Value.Should().Be("ValidNick");
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var cmd = new UpdateNicknameCommand(userId, "ValidNick");

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().BeOfType<NotFoundError>();
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidNickname_ShouldReturnValidationError()
    {
        // Arrange
        var user = new User("hashed_id", DateTimeOffset.UtcNow);
        var cmd = new UpdateNicknameCommand(user.Id, "ab");

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().BeOfType<ValidationError>();

        _userRepositoryMock.Verify(x => x.ExistsWithNicknameAsync(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNicknameAlreadyTaken_ShouldReturnConflictError()
    {
        // Arrange
        var user = new User("hashed_id", DateTimeOffset.UtcNow);
        var cmd = new UpdateNicknameCommand(user.Id, "TakenNick");

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(x => x.ExistsWithNicknameAsync("TakenNick", user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.Should().ContainSingle().Which.Should().BeOfType<ConflictError>();
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUpdatingToSameNickname_ShouldSucceed()
    {
        // Arrange
        var user = new User("hashed_id", DateTimeOffset.UtcNow);
        user.UpdateNickname("MyNick");
        var cmd = new UpdateNicknameCommand(user.Id, "MyNick");

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);


        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}