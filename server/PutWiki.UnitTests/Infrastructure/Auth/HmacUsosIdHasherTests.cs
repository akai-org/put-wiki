using Application.Auth;

using FluentAssertions;

using Infrastructure.Auth;

using Microsoft.Extensions.Options;

namespace PutWiki.UnitTests.Infrastructure.Auth;

public class HmacUsosIdHasherTests
{
    private const string TestKey = "secret-hasher-key";

    private static IOptions<UsosOAuthSettings> CreateOptions(string? keyValue)
    {
        var settings = new UsosOAuthSettings
        {
            HashingKey = keyValue!
        };

        return Options.Create(settings);
    }

    [Fact]
    public void Hash_WithSameInput_ShouldAlwaysReturnIdenticalHash()
    {
        // Arrange
        var options = CreateOptions(TestKey);
        var hasher = new HmacUsosIdHasher(options);
        var usosId = "123456";

        // Act
        var hash1 = hasher.Hash(usosId);
        var hash2 = hasher.Hash(usosId);

        // Assert
        hash1.Should().Be(hash2);
        hash1.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public void Hash_WithDifferentInputs_ShouldReturnDifferentHashes()
    {
        // Arrange
        var options = CreateOptions(TestKey);
        var hasher = new HmacUsosIdHasher(options);

        // Act
        var hashUser1 = hasher.Hash("111111");
        var hashUser2 = hasher.Hash("222222");

        // Assert
        hashUser1.Should().NotBe(hashUser2);
    }

    [Fact]
    public void Hash_WithSameIdButDifferentHashingKey_ShouldReturnDifferentHashes()
    {
        // Arrange
        var hasherWithKeyA = new HmacUsosIdHasher(CreateOptions("key-a"));
        var hasherWithKeyB = new HmacUsosIdHasher(CreateOptions("key-b"));
        var usosId = "123456";

        // Act
        var hashA = hasherWithKeyA.Hash(usosId);
        var hashB = hasherWithKeyB.Hash(usosId);

        // Assert
        hashA.Should().NotBe(hashB);
    }
}