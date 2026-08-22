import {
  Table,
  TableHeader,
  TableRow,
  TableHead,
  TableBody,
  TableCell,
} from '@/components/ui/Table';
import { ScrollArea } from '@/components/ui/ScrollArea';

export interface Semester {
  number: number;
  subjects: string[];
}

interface TableOfCoursesProps {
  semesters: Semester[];
}

export default function TableOfCourses({ semesters }: TableOfCoursesProps) {
  if (!semesters.length) {
    return (
      <ScrollArea className="h-52 rounded-md border p-4">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Semestr</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            <TableRow>
              <TableCell colSpan={1}>Brak danych</TableCell>
            </TableRow>
          </TableBody>
        </Table>
      </ScrollArea>
    );
  }

  const maxSubjects = Math.max(0, ...semesters.map((s) => s.subjects.length));
  const rowIndices = Array.from({ length: maxSubjects }, (_, i) => i);

  return (
    <ScrollArea className="h-52 rounded-md border p-4">
      <Table>
        <TableHeader>
          <TableRow>
            <TableCell className="text-center font-bold" colSpan={semesters.length}>
              Przedmioty na kierunku
            </TableCell>
          </TableRow>
          <TableRow>
            {semesters.map((semester) => (
              <TableHead key={semester.number}>Semestr {semester.number}</TableHead>
            ))}
          </TableRow>
        </TableHeader>
        <TableBody>
          {rowIndices.map((rowIndex) => (
            <TableRow key={`row-${rowIndex}`}>
              {semesters.map((semester) => (
                <TableCell key={semester.number}>{semester.subjects[rowIndex] ?? '-'}</TableCell>
              ))}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </ScrollArea>
  );
}
