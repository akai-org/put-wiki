import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/courses/')({
  component: CoursesCatalogPage,
});

function CoursesCatalogPage() {
  return (
    <section className="mx-auto mt-10 w-full max-w-7xl px-8">
      <h1 className="text-4xl font-bold">Przedmioty</h1>
      <p className="mt-2 text-muted-foreground">Katalog przedmiotów będzie dostępny wkrótce.</p>
    </section>
  );
}
