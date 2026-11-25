using System;

using Domain.Lecturers;

using FluentAssertions;

namespace PutWiki.UnitTests.Domain.Lecturers;

public class LecturerSlugTest
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
        var slug = LecturerSlug.Create(name, usosId);

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
        Action act = () => LecturerSlug.Create(name!, usosId!);

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void ToString_ShouldReturnUnderlyingValue()
    {
        // arrange
        var slug = LecturerSlug.Create("Jan Kowalski", "1001");

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
    public void Parse_WithValidSlugFormat_ShouldReturnLecturerSlug(string validSlug)
    {
        // arrange

        // act
        var slug = LecturerSlug.Parse(validSlug);

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
        Action act = () => LecturerSlug.Parse(invalidSlug!);

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("Łukasz Żółć", "1001", "lukasz-zolc-1001")]
    public void Create_WithUppercasePolishCharacters_ShouldSlugifyCorrectly(string name, string usosId, string expectedSlug)
    {
        // arrange

        // act
        var slug = LecturerSlug.Create(name, usosId);

        // assert
        slug.Value.Should().Be(expectedSlug);
    }

    [Fact]
    public void Create_WithOnlySpecialCharactersInName_ShouldThrowArgumentException()
    {
        // arrange

        // act
        Action act = () => LecturerSlug.Create("!!!", "1001");

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_TwoInstancesWithSameValue_ShouldBeEqual()
    {
        // arrange

        // act
        var slug1 = LecturerSlug.Parse("jan-kowalski-1001");
        var slug2 = LecturerSlug.Parse("jan-kowalski-1001");

        // assert
        slug1.Should().Be(slug2);
        (slug1 == slug2).Should().BeTrue();
    }

    [Fact]
    public void Parse_WhenSlugExceedsMaxLength_ShouldThrowArgumentException()
    {
        // arrange
        var longSlug = new string('a', LecturerSlug.MaxLength + 1);

        // act
        Action act = () => LecturerSlug.Parse(longSlug);

        // assert
        act.Should().Throw<ArgumentException>()
            .WithMessage($"*cannot be longer than {LecturerSlug.MaxLength}*");
    }
}