using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ShortLink.Application.Interfaces;

namespace ShortLink.Function;

public class Function1(ILinkService linkService, ILogger<Function1> logger)
{
    [Function("CreateShortLink")]
    public async Task<IActionResult> Create(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "links")] HttpRequest req)
    {
        CreateShortLinkRequest? request;
        try
        {
            request = await JsonSerializer.DeserializeAsync<CreateShortLinkRequest>(
                req.Body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException)
        {
            return new BadRequestObjectResult(new { error = "Request body must be valid JSON." });
        }

        if (string.IsNullOrWhiteSpace(request?.Url))
            return new BadRequestObjectResult(new { error = "The 'url' field is required." });

        try
        {
            var link = await linkService.ShortenLink(request.Url);
            var host = req.Headers["x-forwarded-host"].FirstOrDefault() ?? req.Host.Value;
            var scheme = req.Headers["x-forwarded-proto"].FirstOrDefault() ?? req.Scheme;
            var shortUrl = $"{scheme}://{host}/api/{link.Id}";

            logger.LogInformation("Created short link with code {Code}", link.Id);
            return new CreatedResult(shortUrl, new
            {
                code = link.Id,
                shortUrl,
                originalUrl = link.Url
            });
        }
        catch (ArgumentException ex)
        {
            return new BadRequestObjectResult(new { error = ex.Message });
        }
    }

    [Function("RedirectShortLink")]
    public async Task<IActionResult> Redirect(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{code}")] HttpRequest req,
        string code)
    {
        var link = await linkService.GetByCodeAsync(code);
        if (link is null)
            return new NotFoundResult();

        await linkService.RegisterAccessAsync(code);
        logger.LogInformation("Redirecting short link {Code}", code);
        return new RedirectResult(link.Url!, permanent: false);
    }

    private sealed record CreateShortLinkRequest(string? Url);
}
