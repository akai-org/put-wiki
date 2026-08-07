import { useParams } from '@tanstack/react-router';

export default function LecturerPage() {
  const { slug } = useParams({ from: '/lecturer/$slug' });

  return (
    <div className="flex flex-col gap-6 mx-40">
      <div className="flex flex-row items-start gap-4 justify-between">
        <div>zdj</div>
        <div>prowadzący {slug}</div>
        <div>Spis treści</div>
      </div>

      <div>opis</div>

      <div className="flex flex-row gap-4">
        <div className="flex-1 text-center">lewo</div>
        <div className="flex-1 text-center">prawo</div>
      </div>
    </div>
  );
}
