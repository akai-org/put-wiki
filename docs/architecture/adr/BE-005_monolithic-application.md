# BE-005 Monolithic architecture with logical boundaries

## Status
Proposed

## Context
We are currently starting promising project with four primary business areas identified so far: Users, Lecturers, Opinions, and Courses. We have already adopted Clean Architecture, lightweight Domain-Driven Design (DDD), and simple CQRS to maintain strict technical boundaries between our business logic and infrastructure.

We now need to decide on the high-level deployment and structural boundary of the system. While advanced patterns like microservices or modular monoliths offer excellent long-term scalability and strict domain isolation, we must balance this against our current constraints: a small student team, the need for rapid prototyping, and the reality that our domain boundaries are still evolving and might change as we discover new requirements.

## Decision
We will build the backend as a **standard monolith with logical boundaries** (a single deployment unit and a single database context).

- **Deployment:** the entire backend will be compiled and deployed as a single ASP.NET Core application.
- **Data access:** we will use a single Entity Framework Core `DbContext` to allow for easy querying and joining of data across different business entities during our initial development phases.
- **Organization:** while we won't enforce strict physical compilation boundaries between modules yet, we will organize our Application and Domain layers by feature or domain aggregate (e.g., grouping all Course-related Use Cases, domain entities, and interfaces in a dedicated `Courses` directory) to prepare for future extraction.

## Consequences

### (+):
- **High development velocity:** developers can easily build features that span multiple domains (e.g., displaying a Course alongside its average Opinions and Lecturer details) without writing complex inter-module communication code or event buses.
- **Operational simplicity:** a single deployable unit is trivial to host, scale horizontally (by just spinning up more instances behind a load balancer), and monitor. CI/CD pipelines remain simple and fast.
- **Easy refactoring:** because the domain boundaries are not yet locked behind strict module interfaces, renaming, moving, or merging concepts as we learn more about the university domain is cheap and fast.
- **Paves the way for the future:** Because we are using Clean Architecture [BE-001](docs\architecture\adr\BE-001_dependency-inversion-and-domain-isolation.md), our core logic is already decoupled from the infrastructure. This makes transitioning to a modular monolith much easier later when the project size justifies it.

### (-):
- **Risk of coupling:** without strict compiler-level enforcement, developers might become lazy and tightly couple the Opinions logic directly to the Users logic. This requires strict discipline during code reviews to ensure domain aggregates only reference each other by ID, not by direct object references.
- **Future migration effort:** when the time comes to split into a modular monolith, we will have to carefully untangle any cross-domain database queries that we allowed early on.
- **Single point of failure:** if one feature causes a memory leak or crashes the application, the entire API goes down.

## Alternatives
Considered options and why we rejected them:

1. **Microservices architecture:** Rejected. Absolute overkill for a student project. It introduces immense distributed systems complexity (network latency, distributed tracing, eventual consistency, multiple CI/CD pipelines, container orchestration) that would completely halt our actual feature development.

2. **Modular monolith:** Rejected for now. A true modular monolith requires strict data isolation (separate database schemas or contexts per module) and explicit APIs for modules to talk to each other. Given that our domains (Users, Courses, Opinions, Lecturers) are highly interconnected, implementing this from beginning would introduce too much friction. We will revisit this pattern when specific modules become large enough to warrant physical isolation.

## Links to external or internal resources
- [MonolithFirst by Martin Fowler](https://martinfowler.com/bliki/MonolithFirst.html) - Explains why starting with a monolith is almost always the right choice, even if the end goal is microservices.
- [Modular Monolith: A Gentle Introduction by Daniel Mackay](https://www.dandoescode.com/blog/modular-monolith/a-gentle-introduction) - Outlines the benefits of the Modular Monolith pattern and the strict isolation it requires, which we will use as a reference for our future architectural roadmap.
- [Majestic Monolith by David Heinemeier Hansson](https://m.signalvnoise.com/the-majestic-monolith/) - A defense of the monolithic architecture for small to medium-sized teams.
- [Modular Monoliths • Simon Brown • GOTO 2018](https://www.youtube.com/watch?v=5OjqD-ow8GE)