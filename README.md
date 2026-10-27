# ShortLink

ShortLink is a URL-shortening API built with .NET 8 Azure Functions and MongoDB.

## Current capabilities

- Create a short link from an absolute HTTP/HTTPS URL.
- Redirect a short code to its original URL using HTTP 302.
- Store links in MongoDB.
- Increment the access counter after a successful redirect.
- Validate URL schemes and reject malformed create requests.

## Architecture

- **ShortLink.Domain**: domain entities.
- **ShortLink.Application**: application services and contracts.
- **ShortLink.Infra**: MongoDB context and repositories.
- **ShortLink.Function**: HTTP API hosted on Azure Functions.

## Configuration

Set a MongoDB connection string in local configuration using either:

- `ConnectionStrings:MongoDbConnectionS`
- `MongoDbConnectionS`

For local Azure Functions development, configure this in `ShortLink.Function/local.settings.json` under `Values` or `ConnectionStrings` as appropriate for your environment. Do not commit credentials.

## API

The default Azure Functions route prefix is `api`.

### Create a short link

`POST /api/links`

This endpoint currently uses Azure Functions `AuthorizationLevel.Function`, so provide a function key when calling a deployed app.

Request body:

```json
{
  "url": "https://example.com/some/long/path"
}
```

Successful response: `201 Created`, with the generated code, short URL, and original URL.

### Redirect

`GET /api/{code}`

This endpoint is anonymous so the short link can be shared. Active codes redirect to the original URL with HTTP 302; missing or inactive codes return 404.

## Important limitations

This is an initial MVP, not a production-ready multi-user service yet:

- User registration/login is not exposed through HTTP endpoints.
- Links are currently created with an empty owner ID; per-user ownership and authorization still need to be implemented before private link management.
- Password storage should be upgraded from plain SHA-256 to a password hashing algorithm such as Argon2id or PBKDF2 before enabling authentication.
- Collision retry, rate limiting, comprehensive automated tests, and robust VirusTotal result handling remain to be implemented.
- Configure a custom domain and HTTPS for a user-friendly short-link host.

## Build

From the repository root:

```bash
dotnet restore ShortLink.sln
dotnet build ShortLink.sln --configuration Release
```
