using Microsoft.EntityFrameworkCore;
using URLShortener.API.Helpers;
using URLShortener.API.Models;

namespace URLShortener.API.DataAccess;

public class UrlShortenerRepository
{
    private readonly UrlShortenerDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UrlShortenerRepository(UrlShortenerDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ShortenedUrl?> GetShortenedUrlAsync(string shortCode) 
    {
        var shortenedUrl = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        if (shortenedUrl != null)
        {
            shortenedUrl.ClickCount++;
            _ = _dbContext.SaveChangesAsync();
        }

        return shortenedUrl;
    } 
    

    public async Task<ShortenedUrlResponse> AddShortenedUrlAsync(string originalUrl)
    {
        if (!UrlShortenerHelper.UrlValidate(originalUrl))
        {
            throw new InvalidDataException("original url not correct");
        }

        var shortenedUrl = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);

        if (shortenedUrl == null)
        {
            string shortCode;
            do
            {
                shortCode = UrlShortenerHelper.GenerateShortCode();
            }
            while (await _dbContext.ShortenedUrls.AnyAsync(u => u.ShortUrl == shortCode));

            shortenedUrl = new ShortenedUrl
            {
                OriginalUrl = originalUrl,
                ShortUrl = $"{GetDomainName()}/{shortCode}",
                ShortCode = shortCode,
            };

            _dbContext.ShortenedUrls.Add(shortenedUrl);
            await _dbContext.SaveChangesAsync();
        }

        return new ShortenedUrlResponse
        {
            ShortUrl = shortenedUrl.ShortUrl,
            ShortCode = shortenedUrl.ShortCode,
            OriginalUrl = shortenedUrl.OriginalUrl,
        };
    }

    private string GetDomainName()
    {
        var request = _httpContextAccessor?.HttpContext?.Request;
        var domain = $"{request?.Scheme}://{request?.Host}";
        return domain;
    }
}
