import { useEffect, useState } from 'react';

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card.tsx';
import {
  Table,
  TableCaption,
  TableHeader,
  TableRow,
  TableHead,
  TableBody,
  TableCell,
} from '@/components/ui/table.tsx';

const data_url = '../assets/data/indexes_majors.json';
import indexes from '../assets/data/indexes_majors.json';

interface DegreeCourseSemester {
  number: number;
  subjects: string[];
}
interface DegreeCourseOpinion {
  author: string;
  content: string;
  rating: number;
}
interface DegreeCourseWorstSubject {
  name: string;
  mark: number;
}

interface DegreeCourseData {
  name: string;
  description: string;
  master_degree?: string;
  semesters: DegreeCourseSemester[];
  absolvent_future: string;
  worst_subjects: DegreeCourseWorstSubject[];
  opinions: DegreeCourseOpinion[]; // Dodany opcjonalny klucz dla opinii
}
const fallbackMajor: DegreeCourseData = {
  name: 'Kierunek studiów',
  description: 'Opis kierunku studiów',
  master_degree: '',
  semesters: [],
  absolvent_future: '',
  worst_subjects: [{ name: '', mark: 0 }],
  opinions: [],
};

export default function DegreeCourse() {
  const [major, setMajor] = useState<DegreeCourseData>(fallbackMajor);
  const [loading, setLoading] = useState<boolean>(true); // dodać podstronę ładowania
  const [error, setError] = useState<string | null>(null); // przenieść na podstronę błędu

  async function getData(): Promise<void> {
    setLoading(true);
    try {
      const response = await fetch(data_url, {
        headers: {
          'Content-Type': 'application/json',
          Accept: 'application/json',
        },
      });
      if (!response.ok) {
        throw new Error(`Błąd sieci: ${response.status}`);
      }
      const myJson = await response.json();
      console.log('Dane pobrane pomyślnie:', myJson);
      if (Array.isArray(myJson)) {
        if (myJson.length > 0) {
          setMajor(myJson[0]);
        } else {
          setMajor(fallbackMajor);
        }
      } else {
        setMajor(myJson as DegreeCourseData);
      }
    } catch (err) {
      console.error('Nie udało się pobrać danych:', err);
      setError(err instanceof Error ? err.message : String(err));
    } finally {
      setLoading(false);
    }
  }
  useEffect(function () {
    // Używamy pliku JSON jako domyślnego mocka jeśli jest dostępny,
    // w przeciwnym razie wykonujemy fetch (np. w środowisku produkcyjnym).
    if (Array.isArray(indexes) && indexes.length > 0) {
      setMajor(indexes[0] as DegreeCourseData);
      setLoading(false);
    } else {
      getData();
    }
  }, []);

  if (loading) {
    return <p>Ładowanie danych...</p>;
  }

  if (error) {
    return <p>Wystąpił błąd: {error}</p>;
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>{major.name}</CardTitle>
        <CardDescription>
          <p>{major.description}</p>
        </CardDescription>
      </CardHeader>

      <CardContent>
        <p>{major.master_degree}</p>
      </CardContent>

      <Table>
        <TableCaption>Przedmioty w kierunku {major.name}</TableCaption>
        <TableHeader>
          <TableRow>
            {major.semesters.length > 0 ? (
              major.semesters.map(function (semester) {
                return <TableHead key={semester.number}>Semestr {semester.number}</TableHead>;
              })
            ) : (
              <TableHead>Semestr</TableHead>
            )}
          </TableRow>
        </TableHeader>
        <TableBody>
          {major.semesters.length > 0 ? (
            (function () {
              let maxSubjects = 0;
              for (let i = 0; i < major.semesters.length; i++) {
                if (major.semesters[i].subjects.length > maxSubjects)
                  maxSubjects = major.semesters[i].subjects.length;
              }
              const rows = [];
              for (let i = 0; i < maxSubjects; i++) {
                rows.push(
                  <TableRow key={'row-' + i}>
                    {major.semesters.map(function (semester) {
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
              <TableCell colSpan={major.semesters.length}>Brak danych</TableCell>
            </TableRow>
          )}
        </TableBody>
      </Table>
      <div className="container" style={{ display: 'flex', gap: '20px' }}>
        <div className="left-column">
          <Card>
            <CardHeader>
              <CardTitle>Losy Absolwentów</CardTitle>
            </CardHeader>
            <CardContent>
              <p>{major.absolvent_future}</p>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Najtrudniejsze Przedmioty</CardTitle>
            </CardHeader>
            <CardContent>
              <p>Lista najtrudniejszych przedmiotów w kierunku {major.name}</p>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Najgorzej oceniane Przedmioty</CardTitle>
            </CardHeader>
            <CardContent>
              <ul>
                {major.worst_subjects.length > 0 ? (
                  major.worst_subjects.map(function (subject) {
                    return (
                      <li key={subject.name}>
                        {subject.name} - Średnia ocena: {subject.mark}
                      </li>
                    );
                  })
                ) : (
                  <li>Brak danych</li>
                )}
              </ul>
            </CardContent>
          </Card>
        </div>
        <div className="right-column">
          <Card>
            <CardHeader>
              <CardTitle>Filtry Opinii</CardTitle>
            </CardHeader>
            <CardContent>
              <div />
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Opinie o kierunku</CardTitle>
            </CardHeader>
            <CardContent>
              {major.opinions.length > 0 ? (
                major.opinions.map(function (opinion, index) {
                  return (
                    <Card key={index}>
                      <CardTitle>{opinion.author}</CardTitle>
                      <CardContent>
                        <p>{opinion.content}</p>
                        <p> Ocena: {opinion.rating}</p>
                      </CardContent>
                    </Card>
                  );
                })
              ) : (
                <p>Brak opinii</p>
              )}
            </CardContent>
          </Card>
        </div>
      </div>
    </Card>
  );
}
