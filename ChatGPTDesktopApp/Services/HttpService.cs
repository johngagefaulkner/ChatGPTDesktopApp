using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ChatGPTDesktopApp.Services;

internal static class HttpService
{
    private static HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(30),
        DefaultRequestVersion = new Version(2, 0),
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
    };

    /// <summary>
    /// Gets the singleton instance of the <see cref="HttpClient"/> used for making HTTP requests.
    /// </summary>
    /// <returns></returns>
    public static HttpClient GetHttpClient() => _httpClient;

    /// <summary>
    /// Sends a GET request to the specified URI and returns the response content as a string.
    /// </summary>
    /// <param name="requestUri">The <see cref="Uri"/> to send the GET <see cref="HttpRequestMessage"/> to.</param>
    /// <returns>The response content as a <see cref="string"/>.</returns>
    public static async Task<string> GetStringAsync(string requestUri)
    {
        var response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
