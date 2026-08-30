using Domain.Users;

using FluentAssertions;

namespace PutWiki.UnitTests.Domain;

public class NicknameTests
{
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
    public void Create_WithValidNickname_ShouldSucceed(string nickname)
    {
        // Act
        var result = Nickname.Create(nickname);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(nickname.Trim());
    }

    [Theory]
    [InlineData("ab")]
    [InlineData("a")]
    [InlineData("")]
    public void Create_WithTooShortNickname_ShouldReturnFailure(string nickname)
    {
        // Act
        var result = Nickname.Create(nickname);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Should().BeOfType<NicknameTooShortError>();
    }

    [Fact]
    public void Create_WithTooLongNickname_ShouldReturnFailure()
    {
        // Arrange
        var nickname = new string('a', Nickname.MaxLength + 1);

        // Act
        var result = Nickname.Create(nickname);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Should().BeOfType<NicknameTooLongError>();
    }

    [Fact]
    public void Create_WithExactMaxLength_ShouldSucceed()
    {
        // Arrange
        var nickname = new string('a', Nickname.MaxLength);

        // Act
        var result = Nickname.Create(nickname);

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
    public void Create_WithInvalidFormat_ShouldReturnFailure(string nickname)
    {
        // Act
        var result = Nickname.Create(nickname);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors[0].Should().BeOfType<NicknameInvalidFormatError>();
    }

    [Theory]
    [InlineData("  JanK  ", "JanK")]
    [InlineData("  JanKowalski  ", "JanKowalski")]
    public void Create_WithLeadingAndTrailingWhitespace_ShouldTrimAndSucceed(string nickname, string expected)
    {
        // Act
        var result = Nickname.Create(nickname);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(expected);
    }

    [Fact]
    public void Create_ShouldProduceEqualNicknames_WhenValuesAreTheSame()
    {
        // Arrange
        var result1 = Nickname.Create("TestNick");
        var result2 = Nickname.Create("TestNick");

        // Assert
        result1.Value.Should().Be(result2.Value);
    }
}