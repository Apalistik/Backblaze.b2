namespace Apalistik.Backblaze.B2.Api.Tests.Integration;

public record Configuration(Dictionary<string, BackblazeConfiguration> Backblaze);
public record BackblazeConfiguration(string ApplicationKeyId, string ApplicationKey);