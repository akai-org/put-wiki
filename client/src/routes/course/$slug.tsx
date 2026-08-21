import { createFileRoute, useParams } from '@tanstack/react-router';
import { useMemo } from 'react';
import { useCourse } from '@/hooks/useCourse';
import { calculateAverage } from '@/utils';

export const Route = createFileRoute('/course/$slug')({
  component: Course,
});

function Course() {
  const { slug } = useParams({ from: '/course/$slug' });
  const { data, isLoading, isError } = useCourse(slug);
  const averageRating = useMemo(() => calculateAverage(data?.ratings), [data?.ratings]);
  if (isLoading)
    return <div className="flex items-center justify-center text-7xl text-white">Ładowanie...</div>;

  if (isError)
    return (
      <div className="flex items-center justify-center text-7xl text-white">
        Wystąpił niespodziewany błąd
      </div>
    );
  if (!data) return null;
  return (
    <div className="min-h-screen bg-[#1E1E2E] p-8 md:p-16">
      <div className="mx-auto max-w-full ">
        <header>
          <h1 className="mb-20 text-center text-5xl text-white">{data.name.pl}</h1>
        </header>
        <main className="grid grid-cols-1 gap-5 md:grid-cols-3">
          <section className="col-span-2 min-h-40 rounded-md bg-gray-200 p-6 ">
            <h2>{data.description.pl}</h2>
          </section>
          <section className="col-span-1 min-h-40 rounded-md bg-gray-200 p-6 ">
            <h2>Spis treści</h2>
          </section>
          <section className="col-span-2 flex min-h-40 items-center justify-center rounded-md bg-gray-200 p-6">
            <ul className="list-disc pl-5">
              {data.lecturers.map((lecturer) => (
                <li key={lecturer}>{lecturer}</li>
              ))}
            </ul>
          </section>
          <section className="col-span-1 row-span-2 min-h-40 rounded-md bg-gray-200 p-6">
            {data.reviews.map((review, i) => (
              <p className="mb-2 text-sm " key={`${data.slug}-review-${i}`}>
                {review}
              </p>
            ))}
          </section>
          <section className="col-span-2 min-h-40 rounded-md bg-gray-200 p-6 ">
            <h2>Materiały</h2>
            <ul className="list-disc pl-5">
              {data.materials.map((m) => (
                <li key={m.url}>
                  <a
                    className="text-blue-600 underline"
                    href={m.url}
                    rel="noopener noreferrer"
                    target="_blank"
                  >
                    {m.label}
                  </a>
                </li>
              ))}
            </ul>
          </section>
          <section className="col-span-3 min-h-40 rounded-md bg-gray-200 p-6">
            <h2 className="mb-2">Histogram ocen</h2>
            <p>Liczba ocen: {data.ratings.length}</p>
            <p>
              Średnia:
              {averageRating}
            </p>
          </section>
        </main>
      </div>
    </div>
  );
}
