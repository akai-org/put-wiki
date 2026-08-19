interface AboutLecturerProps {
  description: string;
}

export default function AboutLecturer(props: AboutLecturerProps) {
  return (
    <div className="flex flex-col p-3 rounded-xl bg-primary text-primary-foreground">
      <p className="text-xl">O prowadzącym</p>
      <p>{props.description}</p>
    </div>
  );
}
