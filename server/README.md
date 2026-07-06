# PutWiki - Backend

The server side app written in ASP.NET Core (C#). Data is mainly persisted in relational PostgreSQL database.

You need to install:
- .NET platform (to choose right version see [notes](./README.md#Notes))
- Docker

## Architecture overview

The project follows the principles of **Clean Architecture** that organizes an application into separate layers with clearly defined responsibilities. Moreover dependencies point inward so that core business logic remains independent of UI, infrastructure, and framework.

Moreover we take advantage from lightweight **Domain Driven Design**, basic **Command Query Responsibility Segregation** and develop the app as a monolith (with paying attention to loosely coupled dependencies whenever possible). See our `BE-XXX` (BE - **B**ack**E**nd) ADR-s (Architecture Decision Records) for more details [here](../docs/architecture/adr/).

> [!NOTE]
> **API Documentation:** When running backend in the `Development` environment, interactive API documentation page is available at the `/docs` route.

## How to run it?

To develop locally:

1. Open it as solution (.slnx) with desired IDE.
2. 
```bash
cd server
dotnet tool restore
dotnet restore
dotnet run --project Presentation/Presentation.csproj
```

App will use launch settings from the `Presentation/Properties/launchSettings.json` profile. You can also run it with your IDE.

## Authentication

Our app provides a way to users to authenticate using USOS. It allows us to access academic related data on behalf of user.

### USOS OAuth setup

1. Create a `.env` file in the `server/` directory.
2. Copy values from `.env.example` and fill in your `UsosOAuth__ConsumerKey`, `UsosOAuth__ConsumerSecret` that we provided you and any other variables.

### JWT
here is description of our own token used by frontend to authenticate with backend.

## Configuration

In ASP.NET Core app configuration is loaded in specific order and is read using variety of configuration sources: settings files, environment variables, etc. See [this](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-10.0) for more details.

`ASPNETCORE_ENVIRONMENT` variable defines the runtime environment of our ASP.NET Core app. It can be set either to `Development` or `Production`.

In backend application we utilize the following configuration sources:

- **environment variables (.env file)**: We keep there only app secrets such as API keys.

- **appsettings.json**: stores public configuration that is the same regardless in which environment backend runs (`Development`/`Production` or just locally).

- **appsettings.Development.json**: stores public configuration which is used in `Development` environment. It contains also settings that are used only during local backend development outside of Docker.

- **compose.override.yml**: It injects into Docker container with backend app, variables that contain public settings (e.g. CallbackUrl) that are specific to `Development`, but are different when running backend inside Docker.

- **compose.prod.yml**: It injects into Docker container with backend app, variables that contain public backend settings (e.g. CallbackUrl) and are specific to `Production` environment.

We store a little of configuration in compose files, due to the fact that having them in .env requires from developer to manually fill in them. Also if they were stored in **appsettings.Production.json**, updates would require creating new backend release and deployment, whereas now these changes require only deployment to reload configuration on production machine. 

Additionally there would be no difference between local development outside of Docker (`Development` environment) and previewing backend in `Development` environment inside Docker containers. 

> [!NOTE]
> Environments mentioned in this README refer to ASP.NET Core runtime environments and how to develop server-side app. If you want to read about PutWiki environments, were we host it and how it is deployed, see [deployment docs](../docs/architecture/deployment.md#environments).

## Docker

To work locally besides server you need the database. So having Docker installed on your machine is still essential. Start it with `docker compose up database`.

> [!NOTE]
> First you need to provide environment variables for the database. See steps below:

1. Create a `.env.postgres` file in the root directory of entire PutWiki project.
2. Copy values from `.env.postgres.example`.

For more details about how to run the whole PutWiki inside Docker look [here](../docs/architecture/deployment.md#2-putwiki-preview-in-docker)

> [!IMPORTANT]
> when you want to preview the whole PutWiki with backend running either in `Development` or `Production` environment, .env file needs to be changed, because backend will need to connect to database inside docker network (when developing locally backend runs on host machine directly and connects with database by exposed port). So set `Host=database` and `Port=5432` in connection string of .env.

> [!WARNING]
> Running the whole PutWiki with backend in `Production` environment will use urls (e.g. CallbackUrl) that refer to domain name used to access PutWiki in Internet. It won't redirect to developer's local machine. In this case during backend testing, that should be altered manually and restored after.

## Database migrations
When you change the model (e.g. new field to entity in Domain layer that is projected into SQL config in Infrastructure layer) you must add new migration. For managing migrations use `dotnet ef` CLI tool.

> [!WARNING]
> You must restore dotnet tools before first usage. Make sure that you followed all commands in [How to run it](#how-to-run-it) section to use correct version defined in `dotnet-tools.json`

> [!NOTE]
> If you are not familiar with `dotnet ef` CLI tool we recommend to read this [documentation](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) first.

After adding migration and commiting it into git repo, usually you would apply it to update database schema. We automated this step so you don't have to do anything. When this app starts with runtime environment defined as `Development` (no matter whether with Docker Compose or locally), then all migrations are applied automatically.

However on production this had to be done different. See [this ADR](../docs/architecture/adr/BE-006_applying-database-migrations-on-production.md) for more details. Assuming that your merged PR included new migration you will have to follow [additional step](../docs/architecture/deployment.md#applying-database-migrations-to-production-optional-step) during deployment.

## Testing
We use xUnit, Fluent Assertions nad Moq libraries to cover the core logic. We stick to the arrange, act, assert pattern. We highly recommend reading this: [Unit testing best practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices).

## Notes
> [!IMPORTANT]
> Please note that you need to have .NET platform installed to run this app locally. Download version defined in `global.json` with equal or greater feature band and patch level.

