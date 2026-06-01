# BE-004 Error handling and flow control with Result pattern

## Status
Proposed

## Context
In traditional C# development, it is common to throw exceptions for business validation failures (e.g., `throw new Exception("Nickname is already taken.")`).

However, exceptions in .NET are designed more for exceptional circumstances (like the database server crashing, or running out of memory) or for breaches of contracts and programming errors (using `ArgumentException`, `InvalidOperationException` etc). Using exceptions for standard, expected control flow (like a user providing a bad password) is computationally expensive because the runtime must gather a full system stack trace.

Furthermore, using exceptions for logic scatters `try-catch` blocks throughout the Application layer. It makes the code execution flow unpredictable (like `goto` statements), and forces developers to guess which methods might throw which exceptions.

## Decision
We will use the Result pattern for all expected business errors and domain validations, while strictly retaining exceptions for developer errors and system failures.

- **Explicit returns:** Domain methods and Application Use Cases will never return void. They will return a `Result` or `Result<T>` object. This object will clearly encapsulate either a successful outcome containing data, or an explicit failure state containing an error message.

- **Domain errors (Result):** Domain methods and Application Use Cases will return a `Result` or `Result<T>` object for business rule violations (e.g., "Nickname is taken"). This explicitly communicates failure states to the caller.

- **Developer errors & bugs (exceptions):** we will continue to throw standard exceptions (e.g., `ArgumentNullException`, `ArgumentException`, `InvalidOperationException`) to enforce programming contracts. If a method requires a non-null parameter and receives null, it should throw immediately. These represent bugs in the code, not user errors, and will be caught by global middleware to return a 500 error.

## Consequences
### (+):
- **Better performance:** We completely avoid the massive performance penalty of generating stack traces for normal, everyday business rule violations.

- **Explicit contracts:** Method signatures tell the whole story. If you see `public Result<User> CreateUser(...)`, you know immediately, without looking at the implementation, that this operation can fail.

- **Cleaner application flow:** We eliminate nested, ugly `try-catch` blocks. The code reads sequentially with clean guard clauses (e.g., `var result = course.Enroll(); if (result.IsFailure) return result;`).

### (-):
- **Boilerplate code:** it requires wrapping standard return types in the `Result<T>` generic class, and constantly checking the `.IsFailure` or `.IsSuccess` flags after calling domain methods.

- **Mental distinction required:** Developers must learn the difference between a "domain error" (handled via `Result`) and a "developer/system error" (handled via exceptions).

## Alternatives
Considered options and why we rejected them:

1. Throwing Exceptions for all errors: Rejected. Causes performance hits and forces the Application layer to use `try-catch` blocks as a control flow mechanism, making the code harder to read and maintain.

2. Returning `null` or boolean (`true`/`false`): Rejected. While fast, it fails to provide sufficient context. A `false` return does not explain why an operation failed (e.g., whether the nickname was too short, or already taken), making it impossible to return a helpful message to the user.

## Links to external or internal resources
- [Exceptions for flow control in C# by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/exceptions-for-flow-control/)

- [What is an exceptional situation in code? by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/what-is-exceptional-situation/)

- [Error handling: Exception or Result? by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/error-handling-exception-or-result/) - A deep dive into why exceptions should not be used for expected business failures.
