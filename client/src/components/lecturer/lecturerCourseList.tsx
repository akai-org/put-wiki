import LecturerCourse from './lecturerCourse';

export default function ListLecturerCourses() {
  return (
    <div className="flex flex-col gap-2 p-4 bg-gray-50 rounded-xl">
      <p className="text-xl">Prowadzone przemdioty</p>
      <div>
        <p>obecnie</p>
        <LecturerCourse />
      </div>
      <div>
        <p>Przedmioty prowadzone w poprzednich semstrach</p>
        <LecturerCourse />
      </div>
    </div>
  );
}
