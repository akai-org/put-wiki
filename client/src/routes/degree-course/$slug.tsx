import { createFileRoute, useParams } from '@tanstack/react-router';
import { useDegreeCourse } from '@/hooks/useDegreeCourse';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/Card';
import TableOfSubjects from '@/components/subjects/TableOfSubjects';
import TableOfOpinions from '@/components/opinions/TableOfOpinions';

export const Route = createFileRoute('/degree-course/$slug')({
  component: DegreeCourse,
});

function DegreeCourse() {
  const { slug } = useParams({ from: '/degree-course/$slug' });
  const { data, isLoading, isError } = useDegreeCourse(slug);

  if (isLoading)
    return <div className="flex justify-center items-center text-black text-7xl">Ładowanie...</div>;
  if (isError) {
    //maybe add toast about error here
    return (
      <div className="flex justify-center items-center text-black text-7xl">Wystąpił błąd</div>
    );
  }
  if (!data)
    return (
      <div className="flex justify-center items-center text-black   text-7xl">Brak danych</div>
    );

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-2xl font-bold text-center">{data.name}</CardTitle>
        <CardDescription className="max-w-full mx-auto grid grid-cols-1 md:grid-cols-2 gap-6">
          <CardContent>
            <p>{data.description}</p>
          </CardContent>
          <CardContent>
            <p>{data.masterDegree}</p>
          </CardContent>
        </CardDescription>
      </CardHeader>

      <TableOfSubjects semesters={data.semesters} />

      <div className="max-w-full mx-auto grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="mx-auto left-column">
          <Card>
            <CardHeader>
              <CardTitle>Losy Absolwentów</CardTitle>
            </CardHeader>
            <CardContent>
              <p>{data.absolventFuture}</p>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Najtrudniejsze Przedmioty</CardTitle>
            </CardHeader>
            <CardContent>
              <ul>
                {data.hardestSubjects.length > 0 ? (
                  data.hardestSubjects.map(function (subject) {
                    return <li key={subject.name}>{subject.name}</li>;
                  })
                ) : (
                  <li>Brak danych</li>
                )}
              </ul>
            </CardContent>
          </Card>
          <Card>
            <CardHeader>
              <CardTitle>Najgorzej oceniane Przedmioty</CardTitle>
            </CardHeader>
            <CardContent>
              <ul>
                {data.worstSubjects.length > 0 ? (
                  data.worstSubjects.map(function (subject) {
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
          <TableOfOpinions opinions={data.opinions} />
        </div>
      </div>
    </Card>
  );
}
