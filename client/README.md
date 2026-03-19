# PUTwiki - Client (Frontend)

The PUTwiki client is the user interface layer of the application, built with React and designed for scalability, maintainability, and fast development.

## Architecture overview

**PLACEHOLDER**

To communicate with backend API just use path starting from `/api` to access the desired endpoint. You don't need to provide the host.

## Tech Stack

- **Bun** – Package manager and runtime
- **Vite** – Fast development server and build tool
- **TypeScript** – Static typing
- **React** – UI library
- **Tailwind CSS** – Utility-first styling
- **shadcn/ui** – Accessible and customizable UI components

## How to run (dev)?

```bash

cd client

cp .env.example .env

bun install

bun run dev
```

You can also run the whole put-wiki using Docker to preview production version:

```bash
docker compose up
```

## Notes

> ⚠️ Please note that you need to have bun installed to run this app locally. Use version defined in `package.json` `engines.bun`.
