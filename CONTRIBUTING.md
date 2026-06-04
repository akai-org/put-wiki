# Contributing Guide

## Who Can Join?

The project is developed within the AKAI student research group at Poznan University of Technology.  
👉 [AKAI Discord – project channel](https://discordapp.com/channels/768494845634412624/1449463337837527093)

- **Want to help?** If you're not part of the team but have an idea or found a bug — post on the Discord channel or open an issue (we use issue templates).
- **Want to join the team?** Message us on the `put-wiki` channel on the AKAI server.

---

## Getting Started

The simplest way to run the whole put-wiki is to use Docker:
```bash
docker compose -f compose.yml -f compose.prod.yml up
```

However this is only to preview production version of the app*. If you want to develop you need to complete steps below.

*compose file doesn't include any bind mounts or watching to detect code changes.

### 1. Requirements

Make sure you have installed:

- **Frontend**: Bun
- **Backend**: .NET Platform
- **General**: Docker

Please check which versions are used in client and server READMEs.

### 2. Setup

- Clone the repository.
- Follow the instructions in [client/README.md](client/README.md) and [server/README.md](server/README.md) to run the development environments.

### 3. Git hooks (optional)

If you want to have autoformatting before each commit you should run `bun install` in the project root directory. This step is not required, but highly recommended as our CI pipeline will constantly fail at formatting check step without this setup.

---

## Workflow

We manage and plan the project using a (more or less 😉) agile approach.

A detailed description of the technical workflow (task planning, issues, branching strategy, etc.) can be found here **[git](docs/workflow/git.md)** and here **[workflow](docs/workflow/workflow.md)**

---

### TL;DR

- Pick a task from the **Projects** tab (if you don’t see it, contact us on Discord).
- Create a branch from `main`, e.g. `ID-task-name`, where `ID` is the issue number.
- Work locally and commit frequently.
- Open a Pull Request (PR) to `main`.
- Once all status checks (CI) pass and you receive at least one approval, merge it. CD will deploy changes to production.
- Manually verify the changes in production.
- Done!

---

## Code Standards

We care about clean and maintainable code. Formatting and linting are automated.

- **Frontend**: Prettier + ESLint
- **Backend**: `dotnet format`, .editorconfig and SonarAnalyzer

Configuration details and best practices:  
👉 **[Docs: Code Style Guidelines](docs/workflow/code-style.md)**

---

_Product documentation (vision, scope, requirements) can be found in the [`docs/product/`](docs/product/) folder._
