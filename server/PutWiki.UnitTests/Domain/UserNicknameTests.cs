using System;

using Domain.Users;

using FluentAssertions;

namespace PutWiki.UnitTests.Domain;

public class UserNicknameTests
{
    private readonly User _user;

    public UserNicknameTests()
    {
        _user = new User("hashed_id", DateTimeOffset.UtcNow);
    }

    [Theory]
    [InlineData("Jan")]
    [InlineData("JanKowalski")]
    [InlineData("jan kowalski")]
    [InlineData("Jan-Kowalski")]
    [InlineData("Jan_Kowalski")]
    [InlineData("x1y")]
    [InlineData("123")]
    [InlineData("a1b")]
    [InlineData("Zażółć")]
    [InlineData("ąćę łńó")]
    [InlineData("Kraków123")]
    public void UpdateNickname_WithValidNickname_ShouldSucceed(string nickname)
    {
        // Act
        var result = _user.UpdateNickname(nickname);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _user.Nickname.Should().Be(nickname.Trim());
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("a")]
    [InlineData("")]
    public void UpdateNickname_WithTooShortNickname_ShouldReturnFailure(string nickname)
    {
        // Act
        var result = _user.UpdateNickname(nickname);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Should().BeOfType<NicknameTooShortError>();
    }

    [Fact]
    public void UpdateNickname_WithTooLongNickname_ShouldReturnFailure()
    {
        // Arrange
        var nickname = new string('a', User.MaxNicknameLength + 1);

        // Act
        var result = _user.UpdateNickname(nickname);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Should().BeOfType<NicknameTooLongError>();
    }

    [Fact]
    public void UpdateNickname_WithExactMaxLength_ShouldSucceed()
    {
        // Arrange
        var nickname = new string('a', User.MaxNicknameLength);

        // Act
        var result = _user.UpdateNickname(nickname);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData("Jan@Kowalski")]
    [InlineData("Jan!")]
    [InlineData("Jan#K")]
    [InlineData("Jan.K")]
    [InlineData("user<script>")]
    [InlineData("Jan  Kowalski")]
    [InlineData("a  b")]
    [InlineData("-JanK")]
    [InlineData("_JanK")]
    [InlineData("JanK-")]
    [InlineData("JanK_")]
    public void UpdateNickname_WithInvalidFormat_ShouldReturnFailure(string nickname)
    {
        // Act
        var result = _user.UpdateNickname(nickname);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Should().BeOfType<NicknameInvalidFormatError>();
    }

    [Theory]
    [InlineData("  JanK  ", "JanK")]
    [InlineData("  JanKowalski  ", "JanKowalski")]
    public void UpdateNickname_WithLeadingAndTrailingWhitespace_ShouldTrimAndSucceed(string nickname, string expected)
    {
        // Act
        var result = _user.UpdateNickname(nickname);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _user.Nickname.Should().Be(expected);
    }

    [Fact]
    public void UpdateNickname_ShouldUpdateExistingNickname()
    {
        // Arrange
        _user.UpdateNickname("OldNick");

        // Act
        var result = _user.UpdateNickname("NewNick");

        // Assert
        result.IsSuccess.Should().BeTrue();
        _user.Nickname.Should().Be("NewNick");
    }

    [Fact]
    public void Nickname_ShouldBeNullByDefault()
    {
        // Assert
        _user.Nickname.Should().BeNull();
    }
}
