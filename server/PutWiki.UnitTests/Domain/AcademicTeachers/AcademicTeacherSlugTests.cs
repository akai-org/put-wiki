using System;

using Domain.AcademicTeachers;

using FluentAssertions;

namespace PutWiki.UnitTests.Domain.AcademicTeachers;

public class AcademicTeacherSlugTests
{
    [Theory]
    [InlineData("Jan Kowalski", "uSOs-1001", "jan-kowalski-usos-1001")]
    [InlineData("Natalia Nowak-Nowacka", "1001", "natalia-nowak-nowacka-1001")]
    [InlineData("Krzysztof Zwierzyński", "2002", "krzysztof-zwierzynski-2002")]
    [InlineData("Rafał Walkowiak", "3003", "rafal-walkowiak-3003")]
    public void From_WithValidNameAndUsosId_ShouldReturnCorrectSlug(string name, string usosId, string expectedSlug)
    {
        // arrange

        // act
        var slug = AcademicTeacherSlug.From(name, usosId);

        // assert
        slug.Should().NotBeNull();
        slug.Value.Should().Be(expectedSlug);
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData("", "1001")]
    [InlineData("  ", "1001")]
    [InlineData("Jan Kowalski", null)]
    [InlineData("Jan Kowalski", "")]
    [InlineData("Jan Kowalski", " ")]
    public void From_WithNullOrWhitespaceInputs_ShouldThrowArgumentException(string? name, string? usosId)
    {
        // arrange

        // act
        Action act = () => AcademicTeacherSlug.From(name!, usosId!);

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ToString_ShouldReturnUnderlyingValue()
    {
        // arrange
        var slug = AcademicTeacherSlug.From("Jan Kowalski", "1001");

        // act
        var stringifiedSlug = slug.ToString();

        // assert
        stringifiedSlug.Should().Be("jan-kowalski-1001");
    }
}