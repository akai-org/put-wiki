# Contributing Guide

## Who Can Join?

The project is developed within the AKAI student research group at Politechnika Poznańska.  
👉 [AKAI Discord – project channel](https://discordapp.com/channels/768494845634412624/1449463337837527093)

- **Want to help?** If you're not part of the core team but have an idea or found a bug — post on the Discord channel and open an Issue (we use issue templates).
- **Want to join the team?** Message us on the `putwiki` channel on the AKAI server.

---

## Getting Started

### 1. Requirements

Make sure you have installed:

- **Frontend**: Node.js, Bun
- **Backend**: .NET 10 Platform
- **General**: Docker

### 2. Setup

- Clone the repository.
- Follow the instructions in `client/README.md` and `server/README.md` to run the development environments.

---

## Workflow

We manage and plan the project using a (more or less 😉) agile approach.

A detailed description of the technical workflow (task planning, issue creation, branching strategy, etc.) can be found here **[git](docs/workflow/git.md)** and here **[workflow](docs/workflow/workflow.md)**

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
- **Backend**: `dotnet format` / editorconfig

Configuration details and best practices:  
👉 **[Docs: Code Style Guidelines](docs/workflow/code-style.md)**

---

## Definition of Done

A task is considered complete when:

- [x] All automated tests (CI) pass.
- [x] Code review is approved by at least one team member.
- [x] The feature has been manually tested.
- [x] Documentation has been updated (if required).
- [x] The code is merged into `main` and CD passes.
- [x] Changes are manually verified in production.

---

_Product documentation (vision, scope, requirements) can be found in the [`docs/product/`](docs/product/) folder._
