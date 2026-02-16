# Iteration Flow

Pracujemy w [np. 2-tygodniowych] cyklach (sprintach), podchodząc do tego elastycznie.

## 1. Planning (Początek iteracji)
* Spotykamy się raz na [okres].
* Konsultujemy z ludźmi z zespołu o taskach in progress, blokadach itp.
* Przeglądamy kolumnę **Backlog** na GitHub Projects.
* Wybieramy zadania (issues) do zrobienia w tym cyklu i przesuwamy je do kolumny **Todo**.
* Upewniamy się, że każde zadanie ma jasny opis.

## 2. Development (W trakcie)
1.  Bierzesz zadanie z kolumny **Todo** bez assignee.
2.  Przypisujesz się do issue.
3.  Przesuwasz issue do **In Progress**.
4.  Tworzysz branch wg: [guidelines](/docs/workflow/code-style.md) i zaczynasz pracę.

## 3. Review & Feedback
1.  Wystawiasz PR wg [PR lifecycle](/docs/workflow/git.md).
2.  Przestawiasz task do kolumny In review.
3.  Pingujesz zespół na dc o review (lub dc bot).

## 4. Done
Po zmergowaniu PR-a, issue automatycznie trafia do **Done**.

## brainstorming
link do boarda figmy

## Hierarchia zadań

1.  **Epics:**
    * To są issues z etykietą `type:epic`.
    * Są trzymane w projekcie githubowym w osobnej zakładce, np. Roadmap/View. Trwają zazwyczaj tygodniami/miesiącami.
    * Epic służy jako kontener. Epic linkuje do user stories.

2.  **User stories:**
    * To są issues z etykietą `type:userstory`.
    * Trafiają na kanban.
    * linkują do implementacji (tasks list). Głownie 1/kilka PR-ów, może jak się coś większego wydzieli to też oddzielnie do innego issue.

4.  **Pull Requests:**
    * PR-y nie trafiają na kanbana. Są tylko zlinkowane z issue (user story). Kiedy jest mergowany to issue samo trafia do done. Kiedy task jest bardzo duży można wydzielić jeszcze oddzielne issue, pod którym będą zalinkowane PR-y (raczej rzadki przypadek)