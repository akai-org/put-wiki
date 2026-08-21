# PutWiki - Client (frontend)

The frontend client for PutWiki, built with React, TypeScript and Vite.

## Tech stack

- **Runtime:** Node.js
- **Package manager:** Bun
- **Core:** React 19, Vite, TypeScript
- **Routing & data fetching:** TanStack Router, TanStack Query, Axios
- **Styling & UI:** Tailwind CSS v4, shadcn/ui (Radix UI)
- **Validation:** Zod
- **Testing & component docs:** Vitest, RTL, Playwright, Storybook

## Getting started

### Prerequisites

> [!IMPORTANT]
>
> - **Node.js:** Required runtime (used version: `engines.node` in package.json). Use `nvm use` to use Node.js version defined in `.nvmrc`.
> - **Bun:** Required package manager (used version: `engines.bun` in package.json).

### Local development

```bash
cd client
bun install
bun run dev
```

The app will be available at http://localhost:5173.

There is no .env file at the current stage of development.

### Docker preview

You can also launch the whole PutWiki using Docker to preview development version:

```bash
docker compose up
```

## Directory structure

All application source code resides in `src/`:

```
src/
├── assets/      # Static media assets
├── components/  # Shared components
│   ├── ui/      # Primitive design system components (shadcn/ui)
│   └── stories/ # Storybook files for UI primitives
├── contexts/    # React Context providers
├── features/    # More complex business domain components/modules
├── hooks/       # Custom reusable React hooks
├── lib/         # external libraries configuration
├── routes/      # File-based routes managed by TanStack Router
├── schemas/     # Zod data validation models
├── styles/      # Tailwind configuration with theme
├── tests/       # Test decorators and test configuration utilities
└── utils/       # Shared utility functions, helpers
```

Moreover, some directories contain `__tests__` directory inside for keeping there all unit tests for a component/module.

## Architecture & development conventions

- **Path alias:** use `@/` to import modules relative to the `src` directory (e.g., `@/components/ui/Button`). If you have more complex code try to encapsulate it and provide `index.ts` which will be used from outside (refers mostly to `features/`).
- **API calls:** use paths starting with `/api` when querying backend endpoints. Host prefix should be omitted.
- **Routing:** routes are generated automatically from `src/routes/` into `src/routeTree.gen.ts` by TanStack Router during development.
- **State management:** use contexts for global app state, TanStack Query for managing asynchronous server state and Axios for making HTTP requests to backend API.
- **Storybook:** we use it as a preview and documentation for all UI components and features. Don't use it for whole pages, since it is hard to mock all dependencies this way.

## Main scripts reference

| Script              | Description                                     |
| :------------------ | :---------------------------------------------- |
| `bun run dev`       | Starts Vite development server                  |
| `bun run build`     | Runs TypeScript check and builds for production |
| `bun run test:unit` | Runs unit tests via Vitest                      |
| `bun run test:ui`   | Runs Storybook component visual tests           |
| `bun run storybook` | Starts Storybook                                |
| `bun run lint`      | Runs ESLint and automatically fixes issues      |
| `bun run format`    | Formats code using Prettier                     |

Linting and formatting run automatically in pre-commit githook. CI pipeline also ensures proper code quality.
