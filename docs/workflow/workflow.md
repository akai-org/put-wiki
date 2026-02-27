# Workflow Strategy

Behold our grand plan of making a PUTwiki.

## Work Cycle

We work in flexible, approximately 2-week sprints.

### Meetings & Decision Making

- Hold brief planning/standup meetings to review progress, blockers, and priorities.
- Discuss tasks in progress and confirm goals for the iteration.
- Use Figma board for brainstorming and visual references.

### Reflecting Decisions in Git Project

- Review the **Backlog** on GitHub Projects.
- Move selected issues to **Todo** for the current iteration.
- Ensure each issue has a clear description and acceptance criteria.

### Working on Issues

1. Pick an unassigned issue from **Todo**.
2. Assign it to yourself and move it to **In Progress**.
3. Create a branch following [branching guidelines](/docs/workflow/git.md).
4. Complete the work and prepare a PR.

### Review & Feedback

1. Open a PR following [PR lifecycle](/docs/workflow/git.md).
2. Move the issue to **In Review**.
3. Notify the team for review.
4. After merge, the issue automatically moves to **Done**.

## Project Phases

This is a long-term project developed in sprints. Below are the defined phases, including their goals, scope, and success criteria. Each phase a has 2 month deadline and consist of about 4 sprints.

---

### Phase 1: Project Setup

**Deadline: 06.03.2026**

#### Main Goal

Define a clear product vision supported by structured documentation and design foundations.

#### Scope

- Establish a well-organized repository structure.
- Create a responsive and consistent design system.
- Define the overall project architecture.

#### Success Criteria

- GitHub repository fully initialized and structured.
- Project documentation and reusable templates created.
- Low-fidelity wireframes prepared and delivered to the designer.
- App is contenerised.
- App is deployed.
- CI is implemented.

#### Out of Scope

- Implementation of application features.

---

### Phase 2: Opinion System and student's account implementation

**Deadline: 07.05.2026**

#### Main Goal

Design and implement the opinion (review) system along with student account functionality.

#### Scope

- Implementation of student authentication and verification via USOS.
- Development of the user account system (profile, authentication state, basic permissions).
- Design and implementation of the opinion data model aligned with future project phases.
- Creation of backend API endpoints for CRD (Create, Read, Delete) operations on opinions.

#### Success Criteria

- Students can successfully authenticate and verify their status via USOS.
- A functional user panel is available and accessible after login.
- Opinion CRD operations work correctly and are secured with proper authorization.
- The data structure supports future system expansion without requiring major refactoring.

#### Out of Scope

- Any web development related to courses and lecturers (e.g., browsing, filtering, ranking, or displaying course/lecturer details).
- Advanced moderation systems (e.g., AI-based filtering, reputation scoring).
