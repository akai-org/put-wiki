import type { LecturerCard } from '@/schemas/lecturer/lecturerCard';
export default function LecturerCard({ title, name, photo_url }: LecturerCard) {
  return (
    <div className="flex flex-row gap-2 p-2 pr-5 bg-gray-50 rounded-xl">
      <img src={`${photo_url}`} className="size-50 rounded-full" alt="lecturer face" />
      <div className="flex flex-col">
        <p className="font-bold text-4xl">{title}</p>
        <p className="font-bold text-3xl">{name}</p>
        <div>tagi specjalizacje</div>
      </div>
    </div>
  );
}
