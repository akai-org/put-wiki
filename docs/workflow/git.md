# Git Workflow

We work with Git following **[Trunk-based Development](https://www.atlassian.com/continuous-delivery/continuous-integration/trunk-based-development)**.

## Branching Strategy

1. **`main`**: Code is merged here only after passing CI. This is the production version. The last merge with changes represents the latest deployed version (we have CD).
2. **Feature branches**: Created from `main`.
   - Naming convention: `ID-short-desc`, where ID is the issue ID.

## Commits

We name commit messages according to **[Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)**.

## Pull Request (PR) Lifecycle

1. **Draft**: If you are working but want to trigger CI or show code, create a PR and mark it as Draft using the provided PR template. If you don't want to create a draft pull request, we recommend at least prefixing its title with `WIP:`.
2. **Ready**: When the code is ready, mark it as "Ready for review" and post it on Discord `Review PR` thread on [put-wiki channel](https://discord.com/channels/768494845634412624/1467612224079007855).
3. **Review**: Requires **at least 1 approval** from another team member.
4. **Merge**:
   - Merge only via **Squash and Merge** – other options are blocked for consistency.
   - Why? To keep `main` history linear and clean (one commit per task/feature) and avoid multiple commits like "fix typo", "wip".
   - **NOTE:** only PR creators should merge the changes, not reviewers or anybody else.

## Rules

- Do not commit directly to `main` (anyway it's blocked by branch protection rules).
- Before asking for PR review, update your branch to resolve conflicts and keep it up-to-date (you can use "update branch" button in PR on Github).
- Do not commit any configuration files containing secrets to the Github repository.
