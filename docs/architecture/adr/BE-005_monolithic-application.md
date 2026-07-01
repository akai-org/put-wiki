# BE-005 Monolithic architecture with logical boundaries

## Status
Accepted

## Context
We are currently starting a promising project with four primary business areas identified so far: Users, Lecturers, Opinions, and Courses. We have already adopted Clean Architecture, lightweight Domain-Driven Design (DDD), and simple CQRS to maintain strict technical boundaries between our business logic and infrastructure.

We now need to decide on the high-level deployment and structural boundary of the system. While patterns such as modular monoliths and microservices can provide stronger module isolation and independent scalability, they also introduce additional complexity that may not be justified at our current stage.

Our team is relatively small, the product requirements are still evolving, and the boundaries between business domains are not yet fully understood. We therefore need an architecture that maximizes development speed and simplicity while still allowing future evolution toward stricter modularization if the project grows.

## Decision
We will build the backend as a **standard monolith with logical boundaries** (a single deployment unit and a single database context).

- **Deployment:** the entire backend will be compiled, tested, and deployed as a single ASP.NET Core application.
- **Data access:** we will use a single Entity Framework Core DbContext and a single relational database during the initial phases of development.
- **Organization:** the codebase will be organized by business capabilities and aggregates (e.g., Courses, Users, Opinions, Lecturers) rather than technical layers alone.
- **Domain boundaries:** despite sharing a codebase and database, we will strive to keep business concepts separated and avoid unnecessary coupling between domains.
- **Future evolution:** if specific domains become significantly larger, require independent deployment, or demonstrate clear ownership boundaries, we will evaluate a transition toward a modular monolith.

## Consequences

### (+):
- **High development velocity:** developers can easily build features that span multiple domains (e.g., displaying a Course alongside its average Opinions and Lecturer details) without writing complex inter-module communication code or event buses.
- **Operational simplicity:** a single deployable unit is trivial to host, scale horizontally (by just spinning up more instances behind a load balancer), and monitor. CI/CD pipelines remain simple and fast.
- **Easy refactoring:** because the domain boundaries are not yet locked behind strict module interfaces, renaming, moving, or merging concepts as we learn more about the university domain is cheap and fast.
- **Paves the way for the future:** Because we are using Clean Architecture [BE-001](docs/architecture/adr/BE-001_dependency-inversion-and-domain-isolation.md), our core logic is already decoupled from the infrastructure. This makes transitioning to a modular monolith much easier later when the project size justifies it.

### (-):
- **Risk of accidental coupling:** because all domains exist in the same codebase and database, developers can create dependencies that blur business boundaries if code reviews are not disciplined.
- **Growing maintenance cost:** as the application expands, build times, deployment times, and the complexity of understanding the entire system may increase.
- **Single deployment unit:** even small changes require redeploying the entire application.
- **Single point of failure:** if one feature causes a memory leak or crashes the application, the entire API goes down.

## Alternatives
Considered options and why we rejected them:

1. **Microservices architecture:** Rejected. Absolute overkill for a student project. It introduces immense distributed systems complexity (network latency, distributed tracing, eventual consistency, multiple CI/CD pipelines, container orchestration) that would completely halt our actual feature development.

2. **Modular monolith:** Rejected for now. A modular monolith is a strong architectural option that enforces clear boundaries between modules within a single deployment unit. Given that our domains (Users, Courses, Opinions, Lecturers) are highly interconnected, implementing this from beginning would introduce too much friction and slow down development. We will revisit this pattern when specific modules become large enough and well-understood to warrant physical isolation.

## Links to external or internal resources
- [MonolithFirst by Martin Fowler](https://martinfowler.com/bliki/MonolithFirst.html) - Explains why starting with a monolith is almost always the right choice, even if the end goal is microservices.
- [Modular Monolith: A Gentle Introduction by Daniel Mackay](https://www.dandoescode.com/blog/modular-monolith/a-gentle-introduction) - Outlines the benefits of the Modular Monolith pattern and the strict isolation it requires, which we will use as a reference for our future architectural roadmap.
- [Majestic Monolith by David Heinemeier Hansson](https://m.signalvnoise.com/the-majestic-monolith/) - A defense of the monolithic architecture for small to medium-sized teams.
- [Modular Monoliths • Simon Brown • GOTO 2018](https://www.youtube.com/watch?v=5OjqD-ow8GE)