# Requirements & Epics

> ⚠️ **Note:** Detailed functional requirements (user stories) can be found on our project Kanban in **GitHub Projects** (and in issues).  
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

---

## Non-Functional Requirements

### Architecture & Code
* Follow the guidelines in [docs/workflow/code-style.md](../workflow/code-style.md)  
* Follow the rules in [docs/workflow/git.md](../workflow/git.md)

### Security
* Tokens and session management .
* Protection against SQL injection.

### UX/UI
* Dark Mode support.
* AA WCAG 2.1 accessibility compliance.
* Responsive design.
