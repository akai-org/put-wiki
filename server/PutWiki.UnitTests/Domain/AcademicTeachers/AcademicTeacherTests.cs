using System;
using System.Collections.Generic;

using Domain.AcademicTeachers;

using FluentAssertions;

namespace PutWiki.UnitTests.Domain.AcademicTeachers;

public class AcademicTeacherTests
{
    private static readonly string[] DefaultValidDegrees = ["dr hab.", "inż."];

    public static TheoryData<string[]> InvalidDegreesData =>
    [
        Array.Empty<string>(),
        [""],
        ["   ", ""]
    ];

    [Fact]
    public void Create_WithValidRequiredParameters_ShouldInitializePropertiesAndGenerateGuid()
    {
        // arrange
        const string usosId = "USOS-12345";
        const string name = "Jan Kowalski";
        const string email = "jan.kowalski@university.edu.pl";

        // act
        var teacher = AcademicTeacher.Create(usosId, DefaultValidDegrees, name, email);

        // assert
        teacher.Should().NotBeNull();
        teacher.Id.Should().NotBeEmpty();
        teacher.UsosId.Should().Be(usosId);
        teacher.Name.Should().Be(name);
        teacher.Email.Should().Be(email);
        teacher.Degrees.Should().Equal("dr hab.", "inż.");
        teacher.PhotoUrl.Should().BeNull();
        teacher.PhoneNumber.Should().BeNull();
        teacher.WebsiteUrl.Should().BeNull();
        teacher.Description.Should().BeNull();
    }

    [Fact]
    public void Create_WithOptionalParameters_ShouldSetAllPropertiesCorrectly()
    {
        // arrange
        const string photoUrl = "https://example.com/photo.jpg";
        const string phoneNumber = "+48 123 456 789";
        const string websiteUrl = "https://example.com";
        const string description = "Experienced lecturer";
        string[] degrees = ["dr"];

        // act
        var teacher = AcademicTeacher.Create(
            usosId: "USOS-12345",
            degrees: degrees,
            name: "Anna Nowak",
            email: "anna.nowak@university.edu.pl",
            photoUrl: photoUrl,
            phoneNumber: phoneNumber,
            websiteUrl: websiteUrl,
            description: description);

        // assert
        teacher.PhotoUrl.Should().Be(photoUrl);
        teacher.PhoneNumber.Should().Be(phoneNumber);
        teacher.WebsiteUrl.Should().Be(websiteUrl);
        teacher.Description.Should().Be(description);
    }

    [Theory]
    [InlineData(null, "Jan Kowalski", "jan@university.edu.pl")]
    [InlineData("", "Jan Kowalski", "jan@university.edu.pl")]
    [InlineData("   ", "Jan Kowalski", "jan@university.edu.pl")]
    [InlineData("USOS-1", null, "jan@university.edu.pl")]
    [InlineData("USOS-1", "", "jan@university.edu.pl")]
    [InlineData("USOS-1", "   ", "jan@university.edu.pl")]
    [InlineData("USOS-1", "Jan Kowalski", null)]
    [InlineData("USOS-1", "Jan Kowalski", "")]
    [InlineData("USOS-1", "Jan Kowalski", "   ")]
    public void Create_WithInvalidRequiredStringParameters_ShouldThrowArgumentException(
        string? usosId,
        string? name,
        string? email)
    {
        // arrange
        string[] degrees = ["mgr"];

        // act
        Action act = () => _ = AcademicTeacher.Create(usosId!, degrees, name!, email!);

        // assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Create_WithNullDegrees_ShouldThrowArgumentNullException()
    {
        // arrange
        IReadOnlyList<string> degrees = null!;

        // act
        Action act = () => _ = AcademicTeacher.Create("USOS-1", degrees, "Jan Kowalski", "jan@university.edu.pl");

        // assert
        act.Should().Throw<ArgumentNullException>()
               .WithParameterName(nameof(degrees));
    }

    [Theory]
    [MemberData(nameof(InvalidDegreesData))]
    public void Create_WithEmptyOrWhitespaceDegreesOnly_ShouldThrowArgumentException(string[] degrees)
    {
        // arrange

        // act
        Action act = () => _ = AcademicTeacher.Create("USOS-1", degrees, "Jan Kowalski", "jan@university.edu.pl");

        // assert
        act.Should().Throw<ArgumentException>()
               .WithMessage("Degrees collection must contain at least one non-empty value.*")
               .WithParameterName(nameof(degrees));
    }

    [Fact]
    public void Create_WithMixedValidAndInvalidDegrees_ShouldFilterOutInvalidEntries()
    {
        // arrange
        string[] degrees = ["dr", "   ", "inż.", ""];

        // act
        var teacher = AcademicTeacher.Create("USOS-1", degrees, "Jan Kowalski", "jan@university.edu.pl");

        // assert
        teacher.Degrees.Should().Equal("dr", "inż.");
    }

    [Fact]
    public void Create_WhenExternalListIsMutated_ShouldNotMutateInternalDegrees()
    {
        // arrange
        var externalList = new List<string> { "mgr", "inż." };

        // act
        var teacher = AcademicTeacher.Create("USOS-1", externalList, "Jan Kowalski", "jan@university.edu.pl");
        externalList.Add("prof.");

        // assert
        teacher.Degrees.Should().Equal("mgr", "inż.");
        teacher.Degrees.Should().NotContain("prof.");
    }
}