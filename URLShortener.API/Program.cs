using Microsoft.EntityFrameworkCore;
using URLShortener.API.DataAccess;
using URLShortener.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<UrlShortenerRepository>();
builder.Services.AddDbContext<UrlShortenerDbContext>(options =>
{
    //options.UseInMemoryDatabase(databaseName: "UrlShortenerDb");
    options.UseSqlite(@"Data Source=AppData\UrlShortener.db");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/shorten", async (UrlShortenerRepository urlShortenerHelper, ShortenedUrlReqest reqest) =>
{
    var shortenedUrl = await urlShortenerHelper.AddShortenedUrlAsync(reqest.OriginalUrl);

    return Results.Created($"{shortenedUrl.ShortCode}", shortenedUrl);
});

app.MapGet("/{shortCode}", async (UrlShortenerRepository urlShortenerHelper, string shortCode) =>
{
    var shortenedUrl = await urlShortenerHelper.GetShortenedUrlAsync(shortCode);
    if (shortenedUrl == null)
    {
        return Results.NotFound();
    }
    return Results.Redirect(shortenedUrl.OriginalUrl);
});

app.Run();
