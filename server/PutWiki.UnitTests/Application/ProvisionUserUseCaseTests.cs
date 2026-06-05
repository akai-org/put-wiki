using System;
using System.Threading;
using System.Threading.Tasks;

using Application.Auth;
using Application.DTOs;
using Application.Errors;
using Application.Mappings;
using Application.Users;

using AutoMapper;

using Domain.Users;

using FluentAssertions;

using FluentResults;

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

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingsProfile>();
        }, new NullLoggerFactory());
        IMapper mapper = mapperConfig.CreateMapper();

        _sut = new ProvisionUserUseCase(
            _usosOAuthServiceMock.Object,
            _idHasherMock.Object,
            _userRepositoryMock.Object,
            NullLogger<ProvisionUserUseCase>.Instance,
            mapper
        );
    }

    [Fact]
    public async Task ExecuteAsync_WhenUsosAuthenticationFails_ShouldReturnFailureResult()
    {
        // Arrange
        var token = "token";
        var verifier = "verifier";
        var expectedError = "Invalid credentials";

        _usosOAuthServiceMock
            .Setup(x => x.HandleCallbackAndGetUserAsync(token, verifier, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Fail(new UnauthorizedError(expectedError)));

        // Act
        var result = await _sut.ExecuteAsync(token, verifier, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors[0].Message.Should().Be(expectedError);

        _idHasherMock.Verify(x => x.Hash(It.IsAny<string>()), Times.Never);
        _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
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
            .ReturnsAsync(Result.Ok(usosUserDto));

        _idHasherMock
            .Setup(x => x.Hash(rawUsosId))
            .Returns(hashedUsosId);

        _userRepositoryMock
            .Setup(x => x.GetByHashedUsosIdAsync(hashedUsosId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingUser);

        // Act
        var result = await _sut.ExecuteAsync(token, verifier, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Id.ToString().Should().Be(existingUser.Id.ToString());
        result.Value.HashedUsosId.Should().Be(hashedUsosId);

        _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Never);
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
            .ReturnsAsync(Result.Ok(usosUserDto));

        _idHasherMock
            .Setup(x => x.Hash(rawUsosId))
            .Returns(hashedUsosId);

        _userRepositoryMock
            .Setup(x => x.GetByHashedUsosIdAsync(hashedUsosId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _sut.ExecuteAsync(token, verifier, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.HashedUsosId.Should().Be(hashedUsosId);
        result.Value.Id.Should().NotBe(Guid.Empty.ToString());

        _userRepositoryMock.Verify(x => x.Add(It.Is<User>(u => u.HashedUsosId == hashedUsosId)), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}