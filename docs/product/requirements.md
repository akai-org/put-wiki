# Requirements & Epics

> ⚠️ **Note:** Detailed functional requirements (user stories) can be found on our project Kanban in [**GitHub Projects**](https://github.com/orgs/akai-org/projects/23) (and in issues).  
> The list below defines the main areas the project covers (both functional and non-functional).

## Functional Requirements (Our Epics)

### E01: Displaying Course and Lecturer Information

Accurate information sourced from USOS, course guides, lecturer websites, and other informational databases, all available in one place.

### E02: Student Account

Users verify their identity using their USOS account. This is required to access the platform and allows them to submit anonymous reviews.

### E03: Opinion System

Reviews are submitted by students and relate to courses or lecturers.

### E04: Information Search

A working search engine for the platform.


## Non-Functional Requirements

### Quality attributes
- **Accessibility & UX:** AA WCAG 2.1 compliance, fully responsive design, and dark mode support.
- **Privacy:** Users login with USOS to our app, but must remain anonymous.
- **Availability:** Controlled deployments with a user-friendly maintenance page to avoid breaking user sessions during updates.

### Process & maintainability
- Follow the workflow and code standards defined in [`docs/workflow/code-style.md`](../workflow/code-style.md) and [`docs/workflow/git.md`](../workflow/git.md).
- Automated CI/CD checks must pass for formatting, linting, and unit tests before any merge.
