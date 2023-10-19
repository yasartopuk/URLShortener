using Microsoft.EntityFrameworkCore;

namespace URLShortener.API.DataAccess;

public class UrlShortenerDbContext : DbContext
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    public UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : base(options)
    { }
}
