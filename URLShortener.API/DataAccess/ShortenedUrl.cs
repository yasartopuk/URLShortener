namespace URLShortener.API.DataAccess;

public class ShortenedUrl
{
    public int Id { get; set; }
    public string OriginalUrl { get; set; }
    public string ShortCode { get; set; }
    public string ShortUrl { get; set; }
    public int ClickCount { get; set; }
    public DateTime CreatedAt { get; set; }

    public ShortenedUrl()
    {
        CreatedAt = DateTime.Now;
    }
}
