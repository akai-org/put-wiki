# Contributing Guide

## 🤝 Kto może dołączyć?
Projekt jest rozwijany w ramach koła naukowego AKAI na Politechnice Poznańskiej [link do project-matching/kanal na dc]. 

- **Chcesz pomóc?** Jeśli nie jesteś w zespole projektu, a masz pomysł/znalazłeś buga – napisz na kanale dc i otwórz Issue! (używamy template'ów)
- **Chcesz dołączyć do zespołu?** Napisz na kanale putwiki na serwerze AKAI.

## 🚀 Getting Started

1. **Wymagania**: Upewnij się, że masz zainstalowane: 
   - Frontend: Node.js,
   - Backend: .NET 9 Platform,
   - Ogólne: Docker
2. **Setup**:
   - Sklonuj repozytorium.
   - Postępuj zgodnie z instrukcjami w `client/README.md` oraz `server/README.md`, aby uruchomić środowiska na dev. [lub calosc docker compose]

## Workflow

Prowadzimy i planujemy projekt w podejściu agile - mniej więcej :))

Szczegółowy opis procesu technicznego (planowanie pracy technicznej, tworzenie issues, itp.) znajdziesz tutaj:
👉 **[Git workflow](docs/workflow/git.md)**

Opis na temat planowania ogólnie zakresu projektu, brainstorming, spotkania, itp. znajdziesz tu:
👉 **[iteration flow](docs/workflow/iteration-flow.md)**

### TL;DR:
1. Wybierz zadanie z zakładki **Projects** (jeśli jej nie widzisz zgłoś się na kanale projektu na dc).
2. Stwórz branch od `main`, np.: `ID-nazwa`, gdzie ID to issue id.
3. Pracuj lokalnie i commituj często.
4. Otwórz Pull Request (PR) do `main`.
5. Gdy wszystkie status checki (CI) przejdą i dostaniesz approve'a zmerguj. Wejdzie CD i zmiany trafią na prod. 
6. Zwryfikuj manualnie na prodzie czy wszystko git.
7. Gotowe! 🎉🎉🎉

## 📐 Code Standards

Dbamy o czystość kodu. Formatting i linting mamy zautomatyzowane.
- **Frontend**: Prettier + ESLint.
- **Backend**: dotnet format / editorconfig.

Szczegóły konfiguracji i dobre praktyki:
👉 **[Docs: Code Style Guidelines](docs/workflow/code-style.md)**

##  Definition of Done

Zadanie uznajemy za skończone, gdy:
- [x] Przechodzi wszystkie testy automatyczne (CI).
- [x] Code review zostało zatwierdzone przez min. 1 osobę.
- [x] Funkcjonalność została przetestowana manualnie.
- [x] Dokumentacja została zaktualizowana (jeśli trzeba). 
- [x] Kod jest zmergowany do `main`. CD przechodzi.
- [x] Zmiany zweryfikowane na prodzie manualnie.

---
*Dokumentacja produktowa (wizja, scope, wymagania) znajduje się w folderze [`docs/product/`](docs/product/).*