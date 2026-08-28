# PutWiki - Client (frontend)

The frontend client for PutWiki, built with React, TypeScript and Vite.

## Tech stack

- **Runtime:** Node.js
- **Package manager:** Bun
- **Core:** React 19, Vite, TypeScript
- **Routing & data fetching:** TanStack Router, TanStack Query, Axios
- **Styling & UI:** Tailwind CSS v4, shadcn/ui (Radix UI)
- **Validation:** Zod
- **Testing:** Vitest, RTL, MSW, Storybook, Playwright

## Getting started

### Prerequisites

> [!IMPORTANT]
>
> - **Node.js:** Required runtime (used version: `engines.node` in package.json). Use `nvm use` to use Node.js version defined in `.nvmrc`.
> - **Bun:** Required package manager (used version: `engines.bun` in package.json).

### Local development

```bash
cd client
nvm use # you must have nvm installed on your machine
bun install
bun run dev
```

The app will be available at http://localhost:5173.

There is no .env file at the current stage of development.

### Docker preview

You can also launch the whole PutWiki using Docker to preview development version and do manual integration tests with other app's services:

```bash
docker compose up
```

## Directory structure

All application source code resides in `src/`:

```
src/
├── assets/      # Contains all the static files such as images, fonts, etc.
├── components/  # Shared components used across the entire application
│   ├── ui/      # Primitive design system components (shadcn/ui)
│   └── layout/  # Components purely for app layout, e.g. Footer, Header.
├── contexts/    # React Context providers
├── features/    # Feature based modules. Each directory encapsulates more complex business logic
├── hooks/       # Shared hooks used across the entire application
├── lib/         # 3rd party libraries preconfigured for the application
├── routes/      # File-based routes managed by TanStack Router
├── schemas/     # App-wide Zod data validation models
├── styles/      # Tailwind configuration with theme
├── tests/       # Test utilities, mocks, MSW config and handlers
└── utils/       # Shared utility functions
```

An example feature module can have the following structure:

```
src/features/awesome-feature
|
+-- api         # exported API request declarations and api hooks related to a specific feature
|
+-- assets      # assets folder can contain all the static files for a specific feature
|
+-- components  # components scoped to a specific feature
|
+-- hooks       # hooks scoped to a specific feature
|
+-- types       # typescript types shared within the feature
|
+-- utils       # utility functions for a specific feature
|
+-- __tests__   # unit tests for a specific feature
```

> [!NOTE]
> You don't need all of these folders for every feature module. Only include the ones that are necessary.

Some directories contain `__tests__/` directory inside for keeping there all unit tests for a component/module. The same applies to `stories/`. This is for readability purposes. It's best to keep all tests, stories, etc. related to a specific component in the same place, right next to it.

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
| `bun run test:unit` | Runs unit tests                                 |
| `bun run storybook` | Starts Storybook                                |
| `bun run lint`      | Runs linting and automatically fixes issues     |
| `bun run format`    | Formats code                                    |

See [package.json](./package.json) `scripts` for more details.

Linting and formatting run automatically in pre-commit githook. CI pipeline also ensures proper code quality.
