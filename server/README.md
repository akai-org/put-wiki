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
2.

```bash
cd server

dotnet restore

dotnet run --project Presentation/Presentation.csproj

```

App will use launch settings from the `Presentation\Properties\launchSettings.json` profile. You can also run it with IDE.

You can launch the whole put-wiki using Docker to preview production version:
```bash
docker compose up
```

## Testing
We use xUnit and Fluent Assertions libs to cover the core domain logic. We stick to the arrange, act, assert pattern. [Unit testing best practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices).

## Notes

> ⚠️ Please note that you need to have .NET platform installed to run this app locally. Download version defined in `global.json`.

