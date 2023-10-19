using System.Text;
using System;

namespace URLShortener.API.Helpers;

public static class UrlShortenerHelper
{
    private static readonly Random random = new Random();
    private const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    internal static string GenerateShortCode()
    {
        var shortUrlBuilder = new StringBuilder(6);

        for (int i = 0; i < 6; i++)
        {
            shortUrlBuilder.Append(allowedChars[random.Next(allowedChars.Length)]);
        }

        return shortUrlBuilder.ToString();
    }

    internal static bool UrlValidate(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
