import { Table, TableHeader, TableRow, TableHead, TableBody, TableCell } from '../ui/table.tsx';
import { ScrollArea } from '../ui/scroll-area.tsx';

interface semester {
  number: number;
  subjects: string[];
}

export default function TableOfSubjects({ semesters }: { semesters: semester[] }) {
  return (
    <ScrollArea className="h-52 rounded-md border p-4">
      <Table>
        <TableHeader>Przedmioty w kierunku Informatyka </TableHeader>
        <TableHeader>
          <TableRow>
            {semesters.length > 0 ? (
              semesters.map(function (semester) {
                return <TableHead key={semester.number}>Semestr {semester.number}</TableHead>;
              })
            ) : (
              <TableHead>Semestr</TableHead>
            )}
          </TableRow>
        </TableHeader>
        <TableBody>
          {semesters.length > 0 ? (
            (function () {
              let maxSubjects = 0;
              for (let i = 0; i < semesters.length; i++) {
                if (semesters[i].subjects.length > maxSubjects)
                  maxSubjects = semesters[i].subjects.length;
              }
              const rows = [];
              for (let i = 0; i < maxSubjects; i++) {
                rows.push(
                  <TableRow key={'row-' + i}>
                    {semesters.map(function (semester) {
                      return (
                        <TableCell key={semester.number}>
                          {semester.subjects[i] ? semester.subjects[i] : '-'}
                        </TableCell>
                      );
                    })}
                  </TableRow>
                );
              }
              return rows;
            })()
          ) : (
            <TableRow>
              <TableCell colSpan={semesters.length}>Brak danych</TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
    </ScrollArea>
  );
}
