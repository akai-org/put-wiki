using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.Core;
using Application.DTOs;
using Application.Users;

using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

using Moq;

namespace PutWiki.UnitTests.Application;

public class ProvisionUserUseCaseTests
{
    private readonly Mock<IUsosOAuthService> _usosOAuthServiceMock;
    private readonly Mock<IUsosIdHasher> _idHasherMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly ProvisionUserUseCase _sut;

    public ProvisionUserUseCaseTests()
    {
        _usosOAuthServiceMock = new Mock<IUsosOAuthService>();
        _idHasherMock = new Mock<IUsosIdHasher>();
        _userRepositoryMock = new Mock<IUserRepository>();

        _sut = new ProvisionUserUseCase(
            _usosOAuthServiceMock.Object,
            _idHasherMock.Object,
            _userRepositoryMock.Object,
            NullLogger<ProvisionUserUseCase>.Instance
        );
    }

    [Fact]
    public async Task ExecuteAsync_WhenUsosAuthenticationFails_ShouldReturnFailureResult()
    {
        // Arrange
        var token = "token";
        var verifier = "verifier";
        var expectedError = "Invalid credentials";
        var expectedCode = 401;

        _usosOAuthServiceMock
            .Setup(x => x.HandleCallbackAndGetUserAsync(token, verifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<UsosUserDto>(expectedError, expectedCode));

        // Act
        var result = await _sut.ExecuteAsync(token, verifier, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(expectedError, result.Error);
        Assert.Equal(expectedCode, result.Code);

        _idHasherMock.Verify(x => x.Hash(It.IsAny<string>()), Times.Never);
        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserAlreadyExistsInDb_ShouldReturnExistingUserWithoutCreatingNewOne()
    {
        // Arrange
        var token = "token";
        var verifier = "verifier";
        var rawUsosId = "12345";
        var hashedUsosId = "XYZ_HASHED_ID";
        var existingUser = new User(hashedUsosId);
        var usosUserDto = new UsosUserDto(rawUsosId);

        _usosOAuthServiceMock
            .Setup(x => x.HandleCallbackAndGetUserAsync(token, verifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(usosUserDto));

        _idHasherMock
            .Setup(x => x.Hash(rawUsosId))
            .Returns(hashedUsosId);

        _userRepositoryMock
            .Setup(x => x.GetByHashedUsosIdAsync(hashedUsosId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _sut.ExecuteAsync(token, verifier, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(existingUser.Id.ToString(), result.Value!.Id);
        Assert.Equal(existingUser.HashedUsosId, result.Value.HashedUsosId);

        _userRepositoryMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenUserDoesNotExistInDb_ShouldCreateNewUserAndSaveToDb()
    {
        // Arrange
        var token = "token";
        var verifier = "verifier";
        var rawUsosId = "567890";
        var hashedUsosId = "ABC_HASHED_ID";
        var usosUserDto = new UsosUserDto(rawUsosId);

        _usosOAuthServiceMock
            .Setup(x => x.HandleCallbackAndGetUserAsync(token, verifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(usosUserDto));

        _idHasherMock
            .Setup(x => x.Hash(rawUsosId))
            .Returns(hashedUsosId);

        _userRepositoryMock
            .Setup(x => x.GetByHashedUsosIdAsync(hashedUsosId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _sut.ExecuteAsync(token, verifier, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(hashedUsosId, result.Value.HashedUsosId);
        Assert.NotEqual(Guid.Empty.ToString(), result.Value.Id);

        _userRepositoryMock.Verify(x => x.AddAsync(It.Is<User>(u => u.HashedUsosId == hashedUsosId), It.IsAny<CancellationToken>()), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenDatabaseThrowsException_ShouldCatchExceptionAndReturnFailureResult()
    {
        // Arrange
        var token = "token";
        var verifier = "verifier";
        var rawUsosId = "123456";
        var hashedUsosId = "HASH";

        var usosUserDto = new UsosUserDto(rawUsosId);
        _usosOAuthServiceMock
            .Setup(x => x.HandleCallbackAndGetUserAsync(token, verifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(usosUserDto));

        _idHasherMock
            .Setup(x => x.Hash(rawUsosId))
            .Returns(hashedUsosId);

        _userRepositoryMock
            .Setup(x => x.GetByHashedUsosIdAsync(hashedUsosId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException("Database connection lost"));

        // Act
        var result = await _sut.ExecuteAsync(token, verifier, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(500, result.Code);
        Assert.Equal("Internal database error during user provisioning.", result.Error);
    }
}