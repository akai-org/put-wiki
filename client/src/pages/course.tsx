export default function CoursePage() {
  return (
    <div className="bg-[#1E1E2E] min-h-screen p-8 md:p-16">
      <div className="max-w-full mx-auto ">
        <header>
          <h1 className="text-5xl text-center text-white mb-20">Nazwa przedmiotu</h1>
        </header>
        <main className="grid grid-cols-1 md:grid-cols-3 gap-5">
          <section className="bg-gray-200 rounded-md p-6 min-h-40 col-span-2 flex items-center justify-center">
            <h2>Opis przedmiotu</h2>
          </section>
          <section className="bg-gray-200 rounded-md p-6 min-h-40 col-span-1 flex items-center justify-center">
            <h2>Spis treści</h2>
          </section>
          <section className="bg-gray-200 rounded-md p-6 min-h-40 col-span-2 flex items-center justify-center">
            <h2>Kto prowadzi przedmiot</h2>
          </section>
          <section className="bg-gray-200 rounded-md p-6 min-h-40 row-span-2 col-span-1 flex items-center justify-center">
            <h2>Oceny</h2>
          </section>
          <section className="bg-gray-200 rounded-md p-6 min-h-40 col-span-2 flex items-center justify-center">
            <h2>Linki do materiałów</h2>
          </section>
          <section className="bg-gray-200 rounded-md p-6 min-h-40 col-span-3 flex items-center justify-center">
            <h2>Histogram ocen</h2>
          </section>
        </main>
      </div>
    </div>
  );
}
