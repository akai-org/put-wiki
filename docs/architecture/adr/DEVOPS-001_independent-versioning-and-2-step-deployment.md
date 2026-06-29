# DEVOPS-001 — Independent versioning and 2-step deployment

## Status
Accepted

## Context
PutWiki is developed as a student research group (AKAI) project operating with a zero-budget. For our MVP phase, we are utilizing a privately-owned Raspberry Pi as our physical production server, placed behind a Cloudflare edge proxy. 

Our codebase is structured as a monorepo containing both the frontend (`client/`) and the backend (`server/`). However, our team situation dictate that developers usually specialize and work exclusively on either the frontend or the backend. 

Due to the limited hardware resources of the Raspberry Pi and the need to avoid unnecessary downtimes, we need a deployment strategy that minimizes build times, separates both apps lifecycles, and gives us full control over when the production environment is updated.

## Decision
We have decided to implement **independent versioning** for the `client` and `server` applications and utilize a **2-step deployment process**:

1. **Automated Continuous Delivery (CD):** Merging a pull request to the `main` branch automatically triggers a GitHub Actions pipeline. This pipeline detects which directories (`client/` or `server/`) have changed, bumps their respective versions (git tags), creates a GitHub Release, builds the Docker image(s), and pushes them to the container registry.

2. **Manual deployment trigger:** The actual deployment to the Raspberry Pi does not happen automatically. The feature author must manually trigger the deployment workflow, which runs directly on the Pi (self-hosted Github Runner), pulls the latest (or specified) Docker images, and restarts the containers.

## Consequences

### (+):
- **CD resource efficiency:** We avoid rebuilding and releasing the frontend if only backend code was changed (and vice versa), saving GitHub Actions minutes.
- **Precise rollbacks:** Decoupled versioning allows us to easily revert a faulty client release to a previous version without affecting the stable backend (and vice versa).
- **Controlled downtime:** A manual trigger gives developers full control over the exact moment the production server restarts, avoiding sudden downtimes during peak hours. Also by using maintenance page the user friendly page is displayed during deployment.
- **Hardware optimization:** By building images with Github Runner and only pulling images from container registry instead of building them on the Raspberry Pi, we significantly reduce the CPU and memory load on our physical hardware.

### (-):
- **Manual overhead:** It requires an extra manual click from the developer to push code to production, making the process not fully automated.
- **Human error:** There is a risk that a developer merges a PR and forgets to click the "Deploy" button, leaving the production environment out of sync with the `main` branch.
- **Complexity in tracking:** It might sometimes be slightly confusing for newcomers to determine if the latest code on `main` is actually running on production without checking the GitHub Deployments tab in repo.
- **Github Releases mismatch:** Because the repository has 2 independently versioned applications, the standard GitHub Releases page becomes a chronological mix of frontend and backend tags (e.g., `client-v1.2.0`, `server-v2.0.1`). This fragmentation makes it harder to see the unified state of the system at a glance, and the "Latest release" badge on the repository homepage will only display the most recently updated component, potentially hiding the current version of the other app.

## Alternatives
Considered options and why we rejected them:
1. **Unified versioning:** We considered having one global version for the entire monorepo. We rejected this because a simple CSS fix would require rebuilding the backend image, unnecessary tying both lifecycles together.
2. **Fully automated CD (continous deployment instead of continous delivery):** We didn't implement it due to the additional complexity to determine whether change is deployable or not (like docs change). Moreover we would lose control over when we want to introduce downtime.

## Links to external or internal resources
- [Semantic versioning (SemVer)](https://semver.org/)
- [Continuous Integration vs. Delivery vs. Deployment by Atlassian](https://www.atlassian.com/continuous-delivery/principles/continuous-integration-vs-delivery-vs-deployment)