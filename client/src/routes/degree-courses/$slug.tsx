import { createFileRoute, useParams } from '@tanstack/react-router';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/Card';
import { TableOfOpinions } from '@/features/opinions';
import {
  degreeCourseQueries,
  TableOfCourses,
  useDegreeCourseQuery,
} from '@/features/degree-courses';

export const Route = createFileRoute('/degree-courses/$slug')({
  component: DegreeCoursePage,
  loader: ({ context: { queryClient }, params: { slug } }) => {
    return queryClient.ensureQueryData(degreeCourseQueries.detail(slug));
  },
});

function DegreeCoursePage() {
  const { slug } = useParams({ from: '/degree-courses/$slug' });
  const { data, isLoading, isError } = useDegreeCourseQuery(slug);

  if (isLoading)
    return <div className="flex items-center justify-center text-7xl text-black">Ładowanie...</div>;
  if (isError) {
    //maybe add toast about error here
    return (
      <div className="flex items-center justify-center text-7xl text-black">Wystąpił błąd</div>
    );
  }
  if (!data)
    return (
      <div className="flex items-center justify-center text-7xl   text-black">Brak danych</div>
    );

  return (
    <Card>
      <CardHeader>
        <CardTitle className="text-center text-2xl font-bold">{data.name}</CardTitle>
        <CardDescription className="mx-auto grid max-w-full grid-cols-1 gap-6 md:grid-cols-2">
          <CardContent>
            <p>{data.description}</p>
          </CardContent>
          <CardContent>
            <p>{data.masterDegree}</p>
          </CardContent>
        </CardDescription>
      </CardHeader>

      <TableOfCourses semesters={data.semesters} />

      <div className="mx-auto grid max-w-full grid-cols-1 gap-6 md:grid-cols-2">
        <div className="mx-auto">
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
        <div className="flex-col">
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
