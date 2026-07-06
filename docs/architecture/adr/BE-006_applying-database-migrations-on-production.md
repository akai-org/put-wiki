# BE-006 — Applying database migrations on production

## Status
Accepted

## Context
Our production environment is hosted on a resource-constrained Raspberry Pi (ARM architecture) using Docker Compose. The backend is an ASP.NET Core application utilizing Entity Framework Core and a relational PostgreSQL database. We needed a safe, reliable, and resource-efficient method to apply database schema changes to the production database without causing startup race conditions, exhausting the Pi's hardware resources, or introducing complex build steps.

## Decision
We decided to generate an idempotent SQL script during the backend continuous delivery (CD) pipeline, attach it to the GitHub Release as an asset, and apply it via a separate, manually triggered GitHub Actions workflow using `docker compose exec` against the running production database container.

## Consequences
### (+):
- **Architecture independence:** SQL script is just plain text and can be easily generated without focusing on Github Runner architecture (x64/arm).
- **Zero resource overhead:** Executing the script via `docker compose exec` reuses the existing database container. It requires no additional temporary or not migration containers or SDKs installed on the Raspberry Pi. Except `gh` CLI to download it :)
- **Auditability and versioning:** Storing the `migration.sql` file in GitHub Releases permanently links the exact database state required for that specific backend version, making debugging easier.
- **Safe and simple execution:** Running migrations as a separate manual step aligns with our deployment strategy [DEVOPS-001](./DEVOPS-001_independent-versioning-and-2-step-deployment.md). It allows us to apply schema changes securely while the app is in maintenance mode. Having it as manual simplifies workflow's code and means that it is launched only when necessary. It separates infrastructure from application code deployment.

### (-):
- **Manual overhead:** Developers must remember an extra manual step: they must check if they added any migrations and trigger the workflow for applying db migrations to prod before deploying the application code.
- **Human error risk:** Forgetting to run the migration before deploying a new server version will lead to 5xx application errors due to a schema mismatch.
- **Infrastructure pollution:** GitHub Release pages contain technical assets (`migration.sql`) alongside the standard release notes. This however is not a big issue as Github Releases are used internally only by us. We don't develop public library.

## Alternatives
Considered options and why we rejected them:
1. **Standalone EF Bundle:** Rejected. While a self-contained bundle can run even without the .NET runtime, it embeds the .NET execution engine into the binary. This causes much higher size (in MB) compared to a lightweight, few-kilobyte SQL text file, unnecessarily consuming GitHub Release storage and network bandwidth. Furthermore, a binary executable cannot be piped into a running database container using cat; it would force us to expose database ports to the host or create a temporary container to execute it.
2. **Applying migrations on app startup:** Rejected. This is an anti-pattern for production (in long term). It can cause race conditions if multiple instances start simultaneously, and it requires the application to run with elevated database schema-modification privileges.
3. **Fully automating migrations inside the main deploy workflow:** Rejected. We want to treat infrastructure changes as a separate concern from application code updates. Keeping it manual gives us explicit control.
4. **Passing the script via GitHub Artifacts or embedding in Docker server image:** Rejected. GitHub Artifacts expire and are complicated to use across different workflows. Embedding the script inside the Docker image adds unnecessary bloat to the application container. GitHub Releases provide permanent, decoupled storage.

## Links to external or internal resources
- [Applying Migrations by Microsoft](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli)
- [DEVOPS-001 Independent versioning and 2-step deployment](./DEVOPS-001_independent-versioning-and-2-step-deployment.md)
