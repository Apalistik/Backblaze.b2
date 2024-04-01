using Microsoft.Extensions.Configuration;

namespace Apalistik.Backblaze.B2.Api.Tests.Integration;

public class AuthorizationTests
{
    private readonly Dictionary<string,BackblazeConfiguration> _backblazeSettings;
    public AuthorizationTests()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<Configuration>()
            .Build();

        var config = configuration.Get<Configuration>();
        _backblazeSettings = config.Backblaze;
    }
    
    [Fact]
    public async void Test1()
    {
        var bucketAuth = _backblazeSettings.FirstOrDefault().Value;
        using var httpClient = new HttpClient();
        var result = await httpClient.GetAuthorizeAccount(new(bucketAuth.ApplicationKeyId, bucketAuth.ApplicationKey));

        Assert.NotNull(result);
        Assert.NotEmpty(result.AuthorizationToken);
    }
}