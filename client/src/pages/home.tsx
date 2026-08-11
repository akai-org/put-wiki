export default function HomePage() {
  return (
    <div className="w-full h-full flex flex-col items-center justify-center">
      <h1 className="text-primary font-bold text-5xl">PUT Wiki Home</h1>

      <div className="mt-6 flex flex-col items-center gap-2">
        <a
          href="/course/analiza-i-struktury-danych"
          className="text-secondary hover:text-primary/80 transition-colors"
        >
          Analiza i struktury danych
        </a>

        <a
          href="/lecturers/jan-kowalski"
          className="text-secondary hover:text-secondary/80 transition-colors"
        >
          Jan Kowalski
        </a>
      </div>
    </div>
  );
}
