# BE-001 Architecture style: dependency inversion & domain isolation

## Status
Accepted

## Context
We are starting this project from scratch. The architectural decisions we make today will dictate how quickly we can add features a year from now.

In traditional architectures (e.g. standard layered architecture), the business logic often becomes coupled to data access details. This inevitably leads to a situation where core business rules become entangled with database specifics (like SQL queries or ORM configurations) and HTTP concerns. As a result, the code becomes difficult to unit test, hard to understand, and expensive to change.

Since we are building a long-term software, we need a foundation that prioritizes and protects our core business logic from the very beginning, keeping it isolated from external technologies, frameworks, and infrastructure.

## Decision
We will adopt an architecture based on **dependency inversion** and **domain isolation** (drawing heavily from clean and hexagonal architectures).

The fundamental rule of this architecture is the **dependency rule:** __source code dependencies must always point inward, toward the core business logic.__

To achieve this, we divide our application into four distinct, concentric layers (from the inside out):

1. **Domain layer (the core):** Contains our business rules, aggregates, entities, and value objects.

    - **Rule:** it has zero dependencies on anything outside of itself. It does not know about databases, JSON, or APIs. 
    
    - **NOTE:** we decided to use small library `FluentResults` in our Domain layer. We treat it as a language primitive. More about Result pattern and error handling in this project [here](docs/architecture/adr/BE-004_error-handling-and-flow-control.md)

2. **Application layer:** Contains use cases (app-specific scenarios, e.g `ProvisionUserUseCase`, `UpdateUserNicknameUseCase`) and queries (fast data retrieval). It coordinates data flow between the Domain layer and Infrastructure (simply put it's the orchestrator).

    - **Rule:** it depends only on the Domain layer. When it needs to save data or talk to an external API, it defines an **interface** (e.g., `IUserRepository`). It does not implement this interface.

3. **Infrastructure layer:** this is where the actual technologies live (e.g., EF Core, third-party API clients, file system access, etc).

    - **Rule:** it implements the interfaces defined by the Application layer (this is the __dependency inversion__).

4. **Presentation / API layer:** contains our HTTP controllers, endpoints, and JSON serialization.

    - **Rule:** it receives web requests, validates the HTTP payload, and simply delegates the work to the use cases in the Application layer.

## Consequences
### (+):
- **Delayed infrastructure decisions:** because the core logic relies on interfaces rather than concrete databases, we can for example start writing and testing business features on day 1 using simple in-memory mock repositories, long before we even decide on a database schema.

- **High testability:** since the Domain and Application layers have no dependencies on external frameworks, we can write lightning-fast unit tests for our business rules without needing to spin up a database or web server.

- **Clear boundaries (no spaghetti code):** new developers have a strict, predictable mental model. You cannot accidentally query the database directly from a controller, which prevents the codebase from degrading into a tangled mess as the team scales. Moreover as Domain layer is written purely in C# without framework code it makes easier for new teammates to understand app's core logic.

### (-):
- **Higher initial boilerplate:** for a new project, this architecture requires creating more files upfront. Instead of one class handling everything, a single feature might require a domain entity, a data transfer object (DTO), an interface, use case and a mapping configuration.

- **Steeper learning curve:** new team members accustomed to traditional "fat controllers" or mixing SQL concerns directly into their services will need time to adjust to strictly separated layers.

- **Overhead for simple CRUD:** basic operations that just read or write a single record without any complex business rules will feel over-engineered, as the data still has to travel through all the architectural layers.

## Alternatives
Considered options and why we rejected them:

1. **Traditional n-tier (controller -> service -> repository):** Rejected. In this model, the database is the foundation. While it is faster to set up initially, it tightly couples business logic to the database technology. This encourages an __"anemic domain model"__ (where business objects are just bags of getters/setters with no logic) and forces developers to write slow, fragile integration tests instead of fast unit tests.

2. **Standard MVC (model-view-controller):** Rejected. MVC is primarily a UI delivery pattern, not a system-wide architecture. Using it as the sole blueprint for a modern backend API inevitably leads to bloated controllers and models that mix HTTP validation, database mapping, and core business rules in the same files.

## Links to external or internal resources
- [The Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) - the foundational article explaining the Dependency Rule.

- [Hexagonal Architecture (aka ports and adapters) by Alistair Cockburn](https://alistair.cockburn.us/hexagonal-architecture) - original article that explains the concept of isolating the core logic using interfaces.

- [Common web application architectures by Microsoft](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures) - about monolithic applications, layers and clean architecture

- [Unpacking the Layers of Clean Architecture – Domain, Application, and Infrastructure Services by Daniel Mackay](https://www.dandoescode.com/blog/unpacking-the-layers-of-clean-architecture-domain-application-and-infrastructure-services) - clean architecture overview and what the term __service__ means depending on layer.

- [The Software Architecture Chronicles by Herberto Graça](https://herbertograca.com/2017/07/03/the-software-architecture-chronicles/) - in depth overview about different software architecture starting from the beginning of software engineering. The one that summarizes everything is [here](https://herbertograca.com/2017/11/16/explicit-architecture-01-ddd-hexagonal-onion-clean-cqrs-how-i-put-it-all-together/)

- [Hexagonal Architecture [Onion/Ports & Adapters] by Bartosz Dąbek](https://www.bdabek.pl/architektura-hexagonalna/) - in Polish