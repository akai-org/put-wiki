import { useParams } from '@tanstack/react-router';
import useCourse from '@/hooks/useCourse';

export default function CoursePage() {
  const { courseId } = useParams({ from: '/course/$courseId' });
  const { data, isLoading, isError } = useCourse(courseId);

  if (isLoading)
    return <div className="flex justify-center items-center text-white text-7xl">Ładowanie...</div>;

  if (isError)
    return (
      <div className="flex justify-center items-center text-white text-7xl">
        Wystąpił niespodziewany błąd
      </div>
    );
  if (!data) return null;
  return (
    <div className="bg-[#1E1E2E] min-h-screen p-8 md:p-16">
      <div className="max-w-full mx-auto ">
        <header>
          <h1 className="text-5xl text-center text-white mb-20">{data.name.pl}</h1>
        </header>
        <main className="grid grid-cols-1 md:grid-cols-3 gap-5">
          <section className="bg-gray-200 rounded-md p-6 min-h-40 col-span-2 ">
            <h2>{data.description.pl}</h2>
          </section>
          <section className="bg-gray-200 rounded-md p-6 min-h-40 col-span-1 ">
            <h2>Spis treści</h2>
          </section>
          <section className="bg-gray-200 rounded-md p-6 min-h-40 col-span-2 flex items-center justify-center">
            <ul className="list-disc pl-5">
              {data.lecturers.map((lecturer) => (
                <li key={lecturer}>{lecturer}</li>
              ))}
            </ul>
          </section>
          <section className="bg-gray-200 rounded-md p-6 min-h-40 row-span-2 col-span-1">
            {data.reviews.map((review, i) => (
              <p key={i} className="mb-2 text-sm ">
                "{review}"
              </p>
            ))}
          </section>
          <section className="bg-gray-200 rounded-md p-6 min-h-40 col-span-2 ">
            <h2>Materiały</h2>
            <ul className="list-disc pl-5">
              {data.materials.map((m) => (
                <li key={m.url}>
                  <a
                    href={m.url}
                    className="text-blue-600 underline"
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    {m.label}
                  </a>
                </li>
              ))}
            </ul>
          </section>
          <section className="bg-gray-200 rounded-md p-6 min-h-40 col-span-3">
            <h2 className="mb-2">Histogram ocen</h2>
            <p>Liczba ocen: {data.ratings.length}</p>
            <p>
              Średnia:
              {(data.ratings.reduce((a, b) => a + b, 0) / data.ratings.length).toFixed(2)} / 5
            </p>
          </section>
        </main>
      </div>
    </div>
  );
}
