using System.Net;
using System.Net.Http.Headers;

using Apalistik.Backblaze.B2.Api.Extensions;

namespace Apalistik.Backblaze.B2.Api;

public static class AuthorizationClient
{
    public static async Task<AuthorizeAccountResponse> GetAuthorizeAccount(this HttpClient httpClient, AuthorizeAccountRequest request)
    {
        using var httpRequest = 
            new HttpRequestMessage(HttpMethod.Get, "https://api.backblazeb2.com/b2api/v3/b2_authorize_account");
        
        httpRequest.Headers.Authorization = 
            new AuthenticationHeaderValue("Basic", GetBasicAuthorizationValue(request.ApplicationKeyId, request.ApplicationKey));

        return await httpClient.SendAsync(httpRequest)
            .Handle<AuthorizeAccountResponse>();
    }

    private static string GetBasicAuthorizationValue(string applicationKeyId, string applicationKey) =>
        Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{applicationKeyId}:{applicationKey}"));
}

public class ApiException : Exception
{
    public HttpStatusCode ResponseStatusCode { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}
public record ApiError(int Status, string Code, string Description)
{
    public int Status { get; init; } = Status;
    public string Code { get; init; } = Code;
    public string Description { get; init; } = Description;
}

public record AuthorizeAccountRequest(string ApplicationKeyId, string ApplicationKey)
{
    public string ApplicationKeyId { get; init; } = ApplicationKeyId;
    public string ApplicationKey { get; init; } = ApplicationKey;
}

public record AuthorizeAccountResponse(
    string AccountId,
    ApiInfo ApiInfo,
    int ApplicationKeyExpirationTimestamp,
    string AuthorizationToken
)
{
    public string AccountId { get; init; } = AccountId;
    public ApiInfo ApiInfo { get; init; } = ApiInfo;
    public int ApplicationKeyExpirationTimestamp { get; init; } = ApplicationKeyExpirationTimestamp;
    public string AuthorizationToken { get; init; } = AuthorizationToken;
}

public record ApiInfo(
    StorageApi StorageApi,
    GroupsApi GroupsApi
)
{
    public StorageApi StorageApi { get; init; } = StorageApi;
    public GroupsApi GroupsApi { get; init; } = GroupsApi;
}

public record StorageApi(
    int AbsoluteMinimumPartSize,
    string ApiUrl,
    string BucketId,
    string BucketName,
    string[] Capabilities,
    string DownloadUrl,
    string InfoType,
    string NamePrefix,
    int RecommendedPartSize,
    string S3ApiUrl
)
{
    public int AbsoluteMinimumPartSize { get; init; } = AbsoluteMinimumPartSize;
    public string ApiUrl { get; init; } = ApiUrl;
    public string BucketId { get; init; } = BucketId;
    public string BucketName { get; init; } = BucketName;
    public string[] Capabilities { get; init; } = Capabilities;
    public string DownloadUrl { get; init; } = DownloadUrl;
    public string InfoType { get; init; } = InfoType;
    public string NamePrefix { get; init; } = NamePrefix;
    public int RecommendedPartSize { get; init; } = RecommendedPartSize;
    public string S3ApiUrl { get; init; } = S3ApiUrl;
}

public record GroupsApi(
    string[] Capabilities,
    string GroupsApiUrl,
    string InfoType
)
{
    public string[] Capabilities { get; init; } = Capabilities;
    public string GroupsApiUrl { get; init; } = GroupsApiUrl;
    public string InfoType { get; init; } = InfoType;
}

