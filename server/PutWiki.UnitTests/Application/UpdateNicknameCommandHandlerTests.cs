using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Errors;
using Application.Features.Users.Commands.UpdateNickname;
using Application.Interfaces;

using Domain.Users;

using FluentAssertions;

using FluentResults;

using Microsoft.Extensions.Logging.Abstractions;

using Moq;

namespace PutWiki.UnitTests.Application;

public class UpdateNicknameCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateNicknameCommandHandler _sut;

    public UpdateNicknameCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _sut = new UpdateNicknameCommandHandler(
            _userRepositoryMock.Object,
            NullLogger<UpdateNicknameCommandHandler>.Instance,
            _unitOfWorkMock.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_WithValidAndUniqueNickname_ShouldSucceed()
    {
        // Arrange
        var user = User.Create("hashed_id", DateTimeOffset.UtcNow);
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
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WithInvalidNickname_ShouldReturnValidationError()
    {
        // Arrange
        var user = User.Create("hashed_id", DateTimeOffset.UtcNow);
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
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNicknameAlreadyTaken_ShouldReturnConflictError()
    {
        // Arrange
        var user = User.Create("hashed_id", DateTimeOffset.UtcNow);
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
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUpdatingToSameNickname_ShouldSucceed()
    {
        // Arrange
        var user = User.Create("hashed_id", DateTimeOffset.UtcNow);
        user.UpdateNickname("MyNick");
        var cmd = new UpdateNicknameCommand(user.Id, "MyNick");

        _userRepositoryMock
            .Setup(x => x.GetByIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _sut.ExecuteAsync(cmd, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}