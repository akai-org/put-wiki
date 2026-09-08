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
    public void Create_WithValidNameAndUsosId_ShouldReturnCorrectSlug(string name, string usosId, string expectedSlug)
    {
        // arrange

        // act
        var slug = AcademicTeacherSlug.Create(name, usosId);

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
    public void Create_WithNullOrWhitespaceInputs_ShouldThrowArgumentException(string? name, string? usosId)
    {
        // arrange

        // act
        Action act = () => AcademicTeacherSlug.Create(name!, usosId!);

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ToString_ShouldReturnUnderlyingValue()
    {
        // arrange
        var slug = AcademicTeacherSlug.Create("Jan Kowalski", "1001");

        // act
        var stringifiedSlug = slug.ToString();

        // assert
        stringifiedSlug.Should().Be("jan-kowalski-1001");
    }

    [Theory]
    [InlineData("jan-kowalski-1001")]
    [InlineData("natalia-nowak-nowacka-1001")]
    [InlineData("krzysztof-zwierzynski-2002")]
    [InlineData("123-abc")]
    [InlineData("teacher")]
    public void Parse_WithValidSlugFormat_ShouldReturnAcademicTeacherSlug(string validSlug)
    {
        // arrange

        // act
        var slug = AcademicTeacherSlug.Parse(validSlug);

        // assert
        slug.Should().NotBeNull();
        slug.Value.Should().Be(validSlug);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("Jan-Kowalski-1001")]
    [InlineData("jan_kowalski_1001")]
    [InlineData("jan--kowalski")]
    [InlineData("-jan-kowalski")]
    [InlineData("jan-kowalski-")]
    [InlineData("jan kowalski")]
    [InlineData("jan-kowalski!")]
    public void Parse_WithInvalidInputs_ShouldThrowArgumentException(string? invalidSlug)
    {
        // arrange

        // act
        Action act = () => AcademicTeacherSlug.Parse(invalidSlug!);

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("Łukasz Żółć", "1001", "lukasz-zolc-1001")]
    public void Create_WithUppercasePolishCharacters_ShouldSlugifyCorrectly(string name, string usosId, string expectedSlug)
    {
        // arrange

        // act
        var slug = AcademicTeacherSlug.Create(name, usosId);

        // assert
        slug.Value.Should().Be(expectedSlug);
    }

    [Fact]
    public void Create_WithOnlySpecialCharactersInName_ShouldThrowArgumentException()
    {
        // arrange

        // act
        Action act = () => AcademicTeacherSlug.Create("!!!", "1001");

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_TwoInstancesWithSameValue_ShouldBeEqual()
    {
        // arrange

        // act
        var slug1 = AcademicTeacherSlug.Parse("jan-kowalski-1001");
        var slug2 = AcademicTeacherSlug.Parse("jan-kowalski-1001");

        // assert
        slug1.Should().Be(slug2);
        (slug1 == slug2).Should().BeTrue();
    }
}