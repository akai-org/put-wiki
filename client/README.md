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

### Mocking backend (MSW)

In development mode API requests are intercepted by [MSW](https://mswjs.io) by default, so you don't need to run the backend. Handlers live in `src/tests/__mocks__/` and are shared with unit tests and Storybook.

To work with the real backend instead, create `.env.local` (see `.env.example`) with `VITE_ENABLE_MOCKS=false` and run the server locally. Vite proxies `/api` requests to `http://localhost:7278`.

### Authentication

Users log in with USOS (OAuth 1.0a). Backend keeps the session in an httpOnly `auth_token` cookie containing JWT. The client never reads the token - it's sent automatically with every request (`withCredentials: true`).

| Step             | Request                                                                           | Response                                                                     |
| :--------------- | :-------------------------------------------------------------------------------- | :--------------------------------------------------------------------------- |
| 1. Start login   | browser navigates to `GET /api/auth/login`                                        | `302` to USOS authorize page                                                 |
| 2. USOS callback | USOS redirects the browser to `GET /api/auth/callback?oauth_token&oauth_verifier` | `200` + `Set-Cookie: auth_token=<JWT>; HttpOnly; SameSite=Lax; Path=/` (24h) |
| 3. Current user  | `GET /api/user/profile`                                                           | `200 { userId, isAuthenticated, authenticationType }` or `401` (empty body)  |
| 4. Logout (TODO) | `POST /api/auth/logout` - not implemented in backend yet                          | `204` + cookie removed                                                       |

Not implemented in backend yet: redirect back to the client after callback, logout, refresh token.

With mocks enabled the whole flow is simulated. Page navigation can't be intercepted by MSW, so step 1 goes to the fake USOS page (`/mock-usos`) instead. Clicking "Zezwól" there calls the mocked callback, which sets the `auth_token` cookie with a fake JWT in MSW's cookie jar (`__msw-cookie-store__` in `localStorage`).

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
