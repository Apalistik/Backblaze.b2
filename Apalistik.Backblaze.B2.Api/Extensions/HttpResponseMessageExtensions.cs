using System.Text.Json;

namespace Apalistik.Backblaze.B2.Api.Extensions;

internal static class HttpResponseMessageExtensions
{
    private static JsonSerializerOptions _options = new(JsonSerializerDefaults.Web)
    {
        
    };
    internal static async Task<TResult> Handle<TResult>(this Task<HttpResponseMessage> httpResponseMessage)
    {
        {
            var response = await httpResponseMessage;
            await using var responseContentStream = await response.Content.ReadAsStreamAsync();
            if (response.IsSuccessStatusCode)
            {
                return (await JsonSerializer.DeserializeAsync<TResult>(responseContentStream, _options))!;
            }
            else
            {
                var error = await JsonSerializer.DeserializeAsync<ApiError>(responseContentStream, _options);
                throw new ApiException
                {
                    ResponseStatusCode = response.StatusCode,
                    Code = error!.Code,
                    Description = error.Description
                };
            }
        }
    }
}