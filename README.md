# BeirutStreetWalksApp

A REST API for managing walking routes ("walks") and the regions of Beirut they belong to, secured with JWT authentication and ASP.NET Core Identity roles. The solution also contains a small, work-in-progress ASP.NET Core MVC front end for regions.

## Tech Stack

- .NET 8 (`net8.0`)
- ASP.NET Core Web API and ASP.NET Core MVC
- Entity Framework Core 8 with SQL Server
- ASP.NET Core Identity (`Microsoft.AspNetCore.Identity.EntityFrameworkCore`) with JWT bearer authentication
- AutoMapper 13
- Asp.Versioning 8
- Swashbuckle (Swagger UI)

## Project Structure

| Project | Description |
| --- | --- |
| `BeirutWalksWebApi` | ASP.NET Core Web API: controllers, two EF Core DbContexts (`ApplicationDbContext` for walks/regions/images, `AuthDb` for Identity), repositories, AutoMapper profile, JWT token creation, global exception middleware and EF Core migrations. |
| `BeirutWalksDomains` | Class library with the entity models (`Walks`, `Regions`, `Difficulty`, `Image`), DTOs and the shared `APIResponse` wrapper. |
| `BeirutWalksUI` | ASP.NET Core MVC app with pages to list, add and edit regions by calling the API through `HttpClient`. Work in progress: it does not send a JWT yet, so the protected region endpoints reject its requests. |

## Features

- CRUD endpoints for regions and walks backed by SQL Server.
- Region listing with name filtering, sorting by name and paging (`name`, `sortBy`, `isAcending`, `pageNb`, `pageSize`).
- Walk listing with name filtering.
- User registration and login with ASP.NET Core Identity; login returns a JWT.
- Role-based authorization on regions: `Reader` can read, `Writer` can create, update and delete. Both roles are seeded by the `AuthDb` migration.
- Image upload (`.jpg`, `.jpeg`, `.png`, max 2 MB) saved to the `Images` folder, recorded in the database and served as static files under `/Images`.
- Swagger UI with a Bearer token input (Development environment).
- Global exception-handling middleware.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB, Express or full edition)
- EF Core CLI tools: `dotnet tool install --global dotnet-ef`

### Configuration

Connection strings and JWT settings are in `BeirutWalksWebApi/appsettings.json`:

```json
"ConnectionStrings": {
  "DbConn": "Server=.;Database=BeirutWalksDb;Integrated Security=True;...",
  "AuthConn": "Server=.;Database=BeirutWalksAuthDb;Integrated Security=True;..."
},
"Jwt": {
  "Key": "CHANGE_ME_use_user_secrets_or_env_var_min_32_chars",
  "Issuer": "https://localhost:7045/",
  "Audience": "https://localhost:7045/"
}
```

Adjust `Server=` to your SQL Server instance. The JWT signing key must be at least 32 characters. Do not commit a real key; set it with user secrets or an environment variable instead:

```bash
cd BeirutWalksWebApi
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "<a random string of at least 32 characters>"
```

or

```bash
# Linux/macOS
export Jwt__Key="<a random string of at least 32 characters>"
# Windows PowerShell
$env:Jwt__Key="<a random string of at least 32 characters>"
```

### Create the databases

The API has two DbContexts, each with its own migrations:

```bash
cd BeirutWalksWebApi
dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context AuthDb
```

### Run

```bash
# API (https://localhost:7045, Swagger at /swagger)
cd BeirutWalksWebApi
dotnet run --launch-profile https

# MVC front end (https://localhost:7219), in a second terminal
cd BeirutWalksUI
dotnet run --launch-profile https
```

To call protected endpoints, register a user with the `Reader` and/or `Writer` role, log in, and send the returned token as `Authorization: Bearer <token>`.

## API Endpoints

Base URL: `https://localhost:7045`

### Auth

| Method | Route | Description |
| --- | --- | --- |
| POST | `/api/Auth/register` | Register a user (`username`, `password`, optional `roles`, e.g. `["Reader","Writer"]`). |
| POST | `/api/Auth/login` | Log in and receive a JWT. |

### Regions (requires JWT)

| Method | Route | Role | Description |
| --- | --- | --- | --- |
| GET | `/api/Regions` | Reader | List regions. Query: `name`, `sortBy`, `isAcending`, `pageNb`, `pageSize`. |
| GET | `/api/Regions/{id}` | Reader | Get a region by id. |
| POST | `/api/Regions` | Writer | Create a region. |
| PUT | `/api/Regions/{id}` | Writer | Update a region. |
| DELETE | `/api/Regions/{id}` | Writer | Delete a region. |

### Walks

| Method | Route | Description |
| --- | --- | --- |
| GET | `/api/Walks` | List walks. Query: `name`. |
| GET | `/api/Walks/{id}` | Get a walk by id. |
| POST | `/api/Walks` | Create a walk. |
| PUT | `/api/Walks/{id}` | Update a walk. |
| DELETE | `/api/Walks/{id}` | Delete a walk. |

### Images

| Method | Route | Description |
| --- | --- | --- |
| POST | `/api/Images/upload` | Upload an image (`multipart/form-data`: `File`, `Name`, `Description`). |

## License

This project is licensed under the MIT License. See [LICENSE.txt](LICENSE.txt).
