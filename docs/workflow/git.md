# Git workflow

Pracujemy z gitem wg **[Trunk-based developement](https://www.atlassian.com/continuous-delivery/continuous-integration/trunk-based-development)**.

## 🌳 Branching Strategy

1.  **`main`**: Kod tutaj jest mergowany dopiero jak przejdzie CI. To jest wersja produkcyjna. Ostatni merge ze zmianami jest najnowszą zdeployowaną wersją (mamy CD).
2.  **Feature branches**: Tworzone od `main`.
    * Naming convention: `ID-short-desc`, gdzie ID to issue id.

## 📝 Commits

Nazywamy messages commitów wg **[Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)**. Łatwiej się zorientować czego dotyczą zmiany + przejrzysty changelog.


## 🔀 Pull Request (PR) Lifecycle

1.  **Draft**: Jeśli pracujesz, ale chcesz uruchomić CI lub pokazać kod – wystaw PR oznaczając go jako Draft używając gotowego szablonu do PR-ów.
2.  **Ready**: Kiedy kod jest gotowy, oznacz jako "Ready for review". (discord bot)
3.  **Review**: Wymagamy **min. 1 approvala** od innego członka zespołu.
4.  **Merge**:
    * Mergujemy tylko przez **Squash and Merge** - inne opcje zablokowane dla jednolitości.
    * Dlaczego? Aby historia `main` była liniowa i czysta (jeden commit na jeden task/feature) i nie było wielu commitów typu "fix typo", "wip". [LINK DO ADR]

## 🚨 Zasady
* Nie commitujemy bezpośrednio do `main` (zablokowane przez branch rules).
* Przed wystawieniem PR zrób rebase swojego brancha aby usunąć konflikty lokalnie (można też update branch po pushu dodatkowo).
* Nie wrzucamy do repozytorium plików konfiguracyjnych z sekretami.