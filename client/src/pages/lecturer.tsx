export default function LecturerPage() {
  return (
    <div className="flex flex-col">
      <div className="grid grid-cols-[auto_1fr_auto] items-start gap-8">
        <div>zdjęcie</div>

        <div className="flex flex-col items-center text-center">
          <h1>Prowadzący</h1>
          <div>Tytuł</div>
          <div>Instytut/Katedra/etc</div>
        </div>

        <div className="justify-self-end">spis treści</div>
      </div>

      <div className="grid grid-cols-2 gap-8">
        <div className="flex flex-col gap-6">
          <div>blok info</div>
          <div>kontakt</div>
          <div>przedmioty</div>
          <div>plan</div>
        </div>

        <div className="flex flex-col gap-4">
          <div>filtr opinii</div>
          <div>opinie</div>
        </div>
      </div>
    </div>
  );
}
