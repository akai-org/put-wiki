import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/lecturers/')({
  component: LecturersCatalogPage,
});

function LecturersCatalogPage() {
  return (
    <section className="mx-auto mt-10 w-full max-w-7xl px-8">
      <h1 className="text-4xl font-bold">Prowadzący</h1>
      <p className="mt-2 text-muted-foreground">Katalog prowadzących będzie dostępny wkrótce.</p>
    </section>
  );
}
