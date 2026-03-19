using Domain;

using FluentAssertions;

namespace PutWiki.UnitTests.Domain;

public class DefaultDomainClassTests
{
    [Fact]
    public void Add_ShouldReturnExpectedSum_WhenBothNumbersArePositive()
    {
        // Arrange
        int a = 5;
        int b = 10;

        // Act
        var result = DefaultClass.Add(a, b);

        // Assert
        result.Should().Be(15);
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(-1, 1, 0)]
    [InlineData(0, 0, 0)]
    public void Add_ShouldReturnExpectedSum_WhenGivenVariousIntegerPairs(int a, int b, int expected)
    {
        // Arrange

        // Act
        var result = DefaultClass.Add(a, b);

        // Assert
        result.Should().Be(expected);
    }
}