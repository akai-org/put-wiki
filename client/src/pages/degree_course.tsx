import { useEffect, useState } from 'react';

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card.tsx';
import TableOfSubjects from '@/components/subjects/tableOfSubjects.tsx';
const data_url = '../assets/data/indexes_majors.json';
import indexes from '../assets/data/indexes_majors.json';
import TableOfOpinions from '@/components/opinions/tableOfOpinions';

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
  master_degree: number;
  semesters: DegreeCourseSemester[];
  absolvent_future: string;
  worst_subjects: DegreeCourseWorstSubject[];
  opinions: DegreeCourseOpinion[]; // Dodany opcjonalny klucz dla opinii
}
const fallbackMajor: DegreeCourseData = {
  name: 'Kierunek studiów',
  description: 'Opis kierunku studiów',
  master_degree: 0,
  semesters: [],
  absolvent_future: '',
  worst_subjects: [{ name: '', mark: 0 }],
  opinions: [],
};

export default function DegreeCourse() {
  const [major, setMajor] = useState<DegreeCourseData>(fallbackMajor);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

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
        <CardTitle className="text-2xl font-bold" style={{ textAlign: 'center' }}>
          {major.name}
        </CardTitle>
        <CardDescription className="max-w-full mx-auto grid grid-cols-1 md:grid-cols-2 gap-6">
          <CardContent>
            <p>{major.description}</p>
          </CardContent>
          <CardContent>
            <p>{major.master_degree}</p>
          </CardContent>
        </CardDescription>
      </CardHeader>

      <TableOfSubjects semesters={major.semesters} />

      <div className="max-w-full mx-auto grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="mx-auto left-column">
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
        <div className="flex-col right-column">
          <Card>
            <CardHeader>
              <CardTitle>Filtry Opinii</CardTitle>
            </CardHeader>
            <CardContent>
              <div />
            </CardContent>
          </Card>
          <TableOfOpinions opinions={major.opinions} />
        </div>
      </div>
    </Card>
  );
}
