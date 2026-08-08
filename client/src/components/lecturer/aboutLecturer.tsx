export default function AboutLecturer(props: { description: string }) {
  return (
    <div className="flex flex-col p-3 rounded-xl bg-gray-50">
      <p className="text-xl">O prowadzącym</p>
      <p>{props.description}</p>
    </div>
  );
}
