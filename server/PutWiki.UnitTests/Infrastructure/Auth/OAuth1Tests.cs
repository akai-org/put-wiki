using System.Collections.Generic;

using FluentAssertions;

using Infrastructure.Auth;

namespace PutWiki.UnitTests.Infrastructure.Auth;

public class OAuth1Tests
{
    [Theory]
    [InlineData("Ladies + Gentlemen", "Ladies%20%2B%20Gentlemen")]
    [InlineData("An encoded string!", "An%20encoded%20string%21")]
    [InlineData("Dogs, Cats & Mice", "Dogs%2C%20Cats%20%26%20Mice")]
    public void UrlEncode_ShouldMatchOAuthRules(string input, string expected)
    {
        OAuth1Helper.UrlEncode(input).Should().Be(expected);
    }

    [Fact]
    public void Signature_ShouldMatchRfc5849Example()
    {
        // Arrange
        var url = "http://photos.example.net/photos";
        var parameters = new Dictionary<string, string>
        {
            ["file"] = "vacation.jpg",
            ["size"] = "original",
            ["oauth_consumer_key"] = "dpf43f3p2l4k3l03",
            ["oauth_token"] = "nnch734d00sl2jdk",
            ["oauth_nonce"] = "kllo9940pd9333jh",
            ["oauth_timestamp"] = "1191242096",
            ["oauth_signature_method"] = OAuth1Helper.SignatureMethod,
            ["oauth_version"] = OAuth1Helper.Version,
        };

        // Act
        var baseString = OAuth1Helper.CreateSignatureBaseString("GET", url, parameters);
        var signature = OAuth1Helper.ComputeHmacSha1Signature(baseString, "kd94hf93k423kf44", "pfkkdhi9sl3r4s00");

        // Assert
        baseString
            .Should()
            .Be(
                "GET&http%3A%2F%2Fphotos.example.net%2Fphotos&file%3Dvacation.jpg%26oauth_consumer_key%3Ddpf43f3p2l4k3l03%26oauth_nonce%3Dkllo9940pd9333jh%26oauth_signature_method%3DHMAC-SHA1%26oauth_timestamp%3D1191242096%26oauth_token%3Dnnch734d00sl2jdk%26oauth_version%3D1.0%26size%3Doriginal"
            );
        signature.Should().Be("tR3+Ty81lMeYAr/Fid0kMTYa/WM=");
    }
}