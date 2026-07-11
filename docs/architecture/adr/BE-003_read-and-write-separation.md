# BE-003 Read and write separation

## Status
Accepted

## Context
In [BE-002 ADR](docs/architecture/adr/BE-002_lightweight-domain-driven-design-and-repo-pattern.md), we established that we will use Rich Domain Models and Repositories to guarantee data consistency. This means when we want to modify data (a write), we load a full object into memory, track changes, and validate rules.

However, most of web traffic consists of simple reads - users just looking at data (e.g., viewing a list of courses, looking at a user profile). Using our Repositories and Rich Domain Models for simple UI reads is highly inefficient. It forces Entity Framework to track changes (which consumes memory) and requires us to map complex aggregates down to simple UI objects.

We need a performant way to query data for the UI without polluting our strict, write-focused Domain layer with database join queries and UI-specific properties.

## Decision
We will cleanly separate our read and write execution paths, adopting a simplified version of **Command Query Responsibility Segregation (CQRS)**.

- **Writes (Commands / Use Cases):** When state changes (e.g., submitting a review, registering a user), the request will go through a Use Case, load the Domain Aggregate via the Repository, execute business rules, and save via Entity Framework with change-tracking enabled.

- **Reads (Queries):** When data is only being displayed (e.g., fetching a list of top-rated courses), the request will bypass the Domain layer and Repositories entirely. We will inject the EF Core `DbContext` directly into a Query class, use it in strictly read-only mode (`.AsNoTracking()`), and project query results into flat DTOs using LINQ translated by EF Core.

- **Explicit execution (without MediatR):** we will implement explicit Handler classes (e.g., CreateUserUseCase, GetCourseDetailsQueryHandler) and inject them directly into our API endpoints via Dependency Injection. We will not use in-memory buses like MediatR to avoid unnecessary abstraction at the beginning.

- **Command/Query objects:** handlers will accept a single, strongly-typed wrapper record (e.g., `CreateUserCommand(string Name, string Email)`) rather than a list of primitive parameters. This future-proofs method signatures and encapsulates request data.

## Consequences
### (+):
- **High performance:** Read operations become blazing fast. Bypassing ORM tracking and projecting directly to flat DTOs saves CPU cycles and memory.

- **Separation of concerns:** The UI often requires data aggregated from many tables. By separating reads, we don't have to ruin our core Domain model by adding UI-specific properties just to satisfy the frontend.

- **Simpler code for reads:** Retrieving data becomes a straightforward LINQ-to-SQL query. No complex repository abstractions are needed.

### (-):
- **Model duplication:** we will have separate representations of the same concepts (e.g., a User domain entity for logic, and a UserProfileDto for display).

- **Mental shift** developers must consciously decide whether a new feature modifies state (a Command) or fetches data (a Query) and place the code in the correct logical bucket.

## Alternatives
Considered options and why we rejected them:

1. **MediatR / in-memory message bus:** Rejected. while popular, it obscures the execution path behind reflection/dynamic dispatch. Explicitly injecting Use Case classes makes the codebase easier to trace and debug for a team just starting out.

2. **Full CQRS with Event Sourcing & separate read database:** Rejected. Over-engineering for a greenfield project. Maintaining two separate physical databases (one for writes, one synchronized for reads) introduces eventual consistency and infrastructure complexity we do not need right now.

## Links to external or internal resources
- [CQRS Pattern by Microsoft](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs) - explains the separation of read and write data stores or models. Also includes section about event sourcing (we won't use it in our project).