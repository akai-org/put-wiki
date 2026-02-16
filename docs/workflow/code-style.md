# Code Style Guidelines

## General rules
1. We try to make our code clean.
2. We adhere to naming conventions specific to the technology in use (platform, language, frameworks/libraries). All code and documentation must be in English only.

## Client-side
We utilize pre-configured `ESLint`[config](client/eslint.config.js) and `Prettier` [config](client/.prettierrc).

**Before committing:**
A script managed by `husky` (pre-commit git hook) will trigger auto-formatting and attempt to fix code issues automatically. In the event of critical errors, the commit will be rejected. Even if you push by bypassing local hooks, the `CI pipeline` will still reject the changes.

## Server-side
Formatting is enforced via the `.editorconfig` [config](server/.editorconfig)
