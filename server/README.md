# PUTwiki - Backend

The server side app written in ASP.NET Core (C#).

You need to install:
- .NET platform (to choose right version see [notes](./README.md#Notes))
- Docker

## Architecture overview

The project follows the principles of **Clean Architecture**, separating concerns into distinct layers. We take advantage also from lightweight **Domain Driven Design**, basic **Command Query Responsibility Segregation** and develop the app as a monolith (with paying attention to loosely couple dependencies whenever possible). See our `BE-XXX` (BE - BackEnd) ADR-s (Architecture Decision Records) for more details [here](../docs/architecture/adr/).

> [!NOTE]
> **API Documentation:** When running in the development environment, interactive API documentation page is available at the `/docs` route.

## How to run it?

To develop locally:

1. Open it as solution (.slnx) with desired IDE.
2. `cd server` and `dotnet tool restore`
3.

```bash
dotnet restore

dotnet run --project Presentation/Presentation.csproj
```

App will use launch settings from the `Presentation/Properties/launchSettings.json` profile. You can also run it with your IDE UI.

## USOS OAuth setup

1. Create a `.env` file in the `server/` directory.
2. Copy values from `.env.example` and fill in your `UsosOAuth__ConsumerKey`, `UsosOAuth__ConsumerSecret` that we provided you and any other variables.

Also make sure that hostname and port for `CallbackUrl` match values for your enviroment (dev, prod). See appsettings.{Enviroment}.json files.

## Docker

To work locally besides server you need the database. So having Docker installed on your machine is still essential. Start it with `docker compose up database`.

**NOTE** you need to provide enviroment variables for the database.
1. Create a `.env.postgres` file in the root directory of entire put-wiki project.
2. Copy values from `.env.postgres.example`.

You can launch the whole put-wiki using Docker to preview development version:
```bash
docker compose up
```

and preview production with:
```bash
docker compose -f compose.yml -f compose.prod.yml up
```

> Note that running development and production versions of the whole put-wiki with docker compose is only for preview, not development.
> 
> In addition, when switching between local development and dev/prod in docker compose, you must update the values of certain environment variables accordingly.

## Testing
We use xUnit and Fluent Assertions libraries to cover the core logic. We stick to the arrange, act, assert pattern. We highly recommend reading this: [Unit testing best practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices).

## Notes

> ⚠️ Please note that you need to have .NET platform installed to run this app locally. Download version defined in `global.json`.

