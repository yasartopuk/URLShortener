namespace URLShortener.API.Models;

public class ShortenedUrlResponse
{
    public string? OriginalUrl { get; set; }
    public string? ShortCode { get; set; }
    public string? ShortUrl { get; set; }
}
