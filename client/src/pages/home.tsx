export default function HomePage() {
  return (
    <div className="w-full h-full flex flex-col items-center justify-center">
      <h1 className=" font-bold text-5xl">PUT Wiki Home</h1>

      <div className="mt-6 p-6 flex flex-col items-center gap-2 bg-primary">
        <a
          href="/course/analiza-i-struktury-danych"
          className="text-primary-foreground transition-colors hover:text-accent"
        >
          Analiza i struktury danych
        </a>

        <a
          href="/lecturers/jan-kowalski"
          className="text-primary-foreground hover:text-primary/80 transition-colors"
        >
          Jan Kowalski
        </a>
      </div>
    </div>
  );
}
