# PUTwiki - Backend

The server side app written in ASP.NET Core (C#).

## Architecture overview

The project follows the principles of **Clean Architecture**, separating concerns into distinct layers:

* **Domain:** Contains enterprise logic, core entities, and custom exceptions. It has no dependencies on other layers or external frameworks.
* **Application:** Holds the business logic, use cases, and interfaces. It orchestrates the flow of data to and from the Domain layer.
* **Infrastructure:** Implements external concerns like database access (Entity Framework Core), Identity, and third-party integrations.
* **Presentation:** The entry point of the application (Web API). It contains controllers, global error handling, validation and API endpoints.

> [!NOTE]
> **API Documentation:** When running in the development environment, interactive API documentation is available at the `/docs` endpoint.

## How to run (dev)?

To develop locally:

1. Open it as solution (.slnx) with desired IDE.
2. `cd server` and `dotnet tool restore`
3.

```bash
dotnet restore

dotnet run --project Presentation/Presentation.csproj
```

App will use launch settings from the `Presentation\Properties\launchSettings.json` profile. You can also run it with your IDE.

## USOS OAuth setup

1. Create a `.env` file in the `server/` directory.
2. Copy values from `.env.example` and fill in your `UsosOAuth__ConsumerKey` and `UsosOAuth__ConsumerSecret`.

Also make sure that hostname and port for `CallbackUrl` match values for your enviroment (dev, prod). See appsettings.{Enviroment}.json files.

## Docker

To work locally besides server you need the database. Start it with `docker compose up database`.

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
We use xUnit and Fluent Assertions libs to cover the core domain logic. We stick to the arrange, act, assert pattern. [Unit testing best practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices).

## Notes

> ⚠️ Please note that you need to have .NET platform installed to run this app locally. Download version defined in `global.json`.

