import type { LecturerCard } from '@/schemas/lecturer/lecturerCard';
export default function LecturerCard({ title, name, surname }: LecturerCard) {
  return (
    <div className="flex flex-row gap-2 p-2 pr-5 bg-gray-50 rounded-xl">
      <img
        src="https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fimg.freepik.com%2Fpremium-photo%2Fmen-different-ethnicities-showcase-unique-facial-expressions-emotions_862489-54692.jpg%3Fw%3D996&f=1&nofb=1&ipt=5fb8cb59427f0c22a38802fc4c853ddd315724b0af4a33366072eae97d698ad2"
        className="size-50 rounded-full"
        alt="lecturer face"
      />
      <div className="flex flex-col">
        <p>{title}</p>
        <p>
          {name} {surname}
        </p>
        <div>tagi specjalizacje</div>
      </div>
    </div>
  );
}
