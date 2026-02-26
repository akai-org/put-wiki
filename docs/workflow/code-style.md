# Code Style Guidelines

## 📜 General Rules
1. Maintain code cleanliness (see: *Clean Code: A Handbook of Agile Software Craftsmanship* - Robert C. Martin)
2. Follow naming conventions related to the technology used (platform, language, frameworks/libraries). Use English only.
3. When possible, adhere to principles/guidelines such as KISS, DRY, SOLID, etc.

## 🎨 Frontend
We use a configured `ESLint` [config](https://github.com/akai-org/put-wiki/blob/main/client/eslint.config.js) and `Prettier` [config](https://github.com/akai-org/put-wiki/blob/main/client/.prettierrc).

**Before committing:**  
A shell script managed by `husky` (pre-commit git hook) runs auto-formatting and fixes autofixable lint errors in the code. For more serious errors, the commit will be rejected. If you push while skipping hooks, the CI will still reject the changes.

## ⚙️ Backend
Formatting is enforced via the `.editorconfig` [config](https://github.com/akai-org/put-wiki/blob/main/server/.editorconfig)
