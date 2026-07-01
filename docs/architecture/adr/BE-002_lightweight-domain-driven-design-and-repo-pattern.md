# BE-002 Lightweight domain driven design and repository pattern

## Status
Accepted

## Context
In many standard web applications, business rules (e.g., "A student cannot review a course they haven't taken," or "A university course must have at least one tutor") end up scattered across UI controllers, application services, or even database triggers.

Furthermore, data objects often take the form of an **anemic domain model** - they are just "bags of properties" with public getters and setters (e.g., `user.Status = "Banned"`). This allows any piece of code anywhere in the system to modify the data directly. Over time, this guarantees data inconsistency and bugs, because developers forget to call the required validation checks before changing the state.

We need a systematic approach to model our core business rules directly within the data structures they govern, ensuring that it is significantly harder to create an invalid state through the domain model.

## Decision
We will adopt lightweight **Domain-Driven Design** concepts combined with the **Repository pattern**.

- **Rich domain models:** our core entities will be "smart". They will guard their own rules (invariants). We will strictly forbid public setters on properties. State changes will only be allowed via specific, behavior-rich business methods (e.g., `user.Ban(reason)` instead of `user.Status = "Banned"`).

- **Value Objects:** we will use immutable Value Objects to encapsulate small, coherent pieces of logic. For example, instead of storing a grade as an `int`, we will create a `Grade` value object that internally guarantees it can only ever hold a value between 2.0 and 5.0.

- **Aggregates:** we will group related entities into single transaction boundaries called Aggregates (e.g., a `Course` and its `Reviews` might form one Aggregate).

- **Repository pattern:** we will use Repositories __exclusively__ to save and retrieve these Aggregates from the database. The rest of the application will ask the Repository for an Aggregate, call a business method on it, and tell the Repository to save it.

## Consequences
### (+):
- **Guaranteed consistency:** because there are no public setters, you cannot bypass the business rules. It becomes impossible to create an invalid domain object.

- **High cohesion:** data and the exact logic that modifies it live in the exact same file. This makes debugging incredibly fast.

- **Expressive code (Ubiquitous Language):** The code in Domain layer reads like plain English and uses the language of the university domain (e.g., `course.AssignTutor(tutorId)` instead of `course.TutorId = tutorId`).

### (-):
- **Mapping overhead:** our ORM (EF Core) sometimes struggles to save highly complex, encapsulated domain entities directly to database tables. We may occasionally need to write mapping code to translate our pure domain entity into a database-friendly structure.

- **Performance considerations:** To update a single property (e.g., changing a review status), we must load the entire Aggregate from the database into memory, execute the logic, and save it back. This is slightly slower than executing a raw SQL `UPDATE` statement.

## Alternatives
Considered options and why we rejected them:

1. **Anemic domain model with logic in services:** Rejected. Moving all validation logic to Application layer services makes the entities "dumb". This inevitably leads to code duplication (e.g., two different services both implement the logic to calculate an average score, doing it slightly differently) and missed validations.

2. **Active record pattern:** Rejected. Tying database access methods directly to the entity itself (e.g., `user.SaveToDb()`) violates the Single Responsibility Principle and couples our pure domain logic directly to database concerns.

3. **Full DDD with event sourcing:** Rejected. While powerful, it is massive over-engineering for our current scale. Event sourcing introduces immense operational and debugging complexity (event stores, eventual consistency) that a new project does not need.

## Links to external or internal resources

- [The repository design pattern by Arnaud Langlade](https://www.arnaudlanglade.com/repository-design-pattern/)

- [Anemic Domain Model by Martin Fowler](https://martinfowler.com/bliki/AnemicDomainModel.html) - explains why objects with only getters and setters are an anti-pattern.

- [Design a microservice domain model by Microsoft](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-domain-model) - In-depth guide to implementation of DDD concepts  like Aggregates and Value Objects in .NET. Contains lots of useful references to other resources.

- [DDD Modelling - Aggregates vs Entities: A Practical Guide by Daniel Mackay](https://www.dandoescode.com/blog/ddd-modelling-aggregates-vs-entities)

- [Domain Driven Design: Patterns for Aggregate Creation Mastery](https://www.dandoescode.com/blog/domain-driven-design-patterns-for-aggregate-creation-mastery)

- [Domain-Driven Design by Herberto Graça](https://herbertograca.com/2017/09/07/domain-driven-design/)

- [Domain Driven Design by Bartosz Dąbek](https://www.bdabek.pl/domain-driven-design/) - in Polish

- [Anemic Domain Model vs Rich Domain Model - What is it? by Bartosz Dąbek](https://www.bdabek.pl/anemic-domain-model-vs-rich-domain-model/) - in Polish
