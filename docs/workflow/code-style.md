# Code Style Guidelines

## 📜 Zasady Ogólne
1. Dbamy o czystość kodu (więcej: Clean Code A Handbook of Agile Software Craftsmanship - Robert C. Martin)
2. Trzymamy się konwencji nazewnictwa związanych z daną technologią (platforma, język, frameworki/biblioteki). Używamy tylko angielskiego.
3. Kiedy można stosujemy się do rad/"zasad" jak KISS, DRY, SOLID, itp.

## 🎨 Frontend
Używamy skonfigurowanego `ESLint` ([config])() oraz `Prettier` ([config])().

**Przed commitem:**
Skrypt shellowy obsługiwany przez `husky` (pre-commit git hook) uruchomi autoformatting i naprawi autofixable lint errors w kodzie. W przypadku poważniejszych błędów odrzuci commita. Jeśli pushujesz z pominięciem hooków to CI i tak odzrzuci zmiany.

## ⚙️ Backend
Formatting wymuszany przez plik `.editorconfig` ([config])()
