import { useParams } from '@tanstack/react-router';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/Card';
import { TableOfOpinions } from '@/features/opinions';
import { degreeCourseQueries, TableOfCourses } from '@/features/degree-courses';
import { useSuspenseQuery } from '@tanstack/react-query';
import createRoute from '@/utils/createRoute';

export const Route = createRoute(
  '/degree-courses/$slug',
  DegreeCoursePage,
  DegreePageSkeleton,
  degreeCourseQueries
);

function DegreeCoursePage() {
  const { slug } = useParams({ from: '/degree-courses/$slug' });
  const { data } = useSuspenseQuery(degreeCourseQueries.bySlug(slug));

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

function DegreePageSkeleton() {
  return (
    <div aria-label="Loading lecturer" className="mx-auto mt-4 w-full max-w-7xl animate-pulse">
      <div className="grid grid-cols-1 self-stretch md:grid-cols-[7fr_3fr]">
        <div className="m-3 h-56 rounded-xl bg-muted" />
        <div className="m-3 h-56 rounded-xl bg-muted" />
      </div>
      <div className="m-3 h-40 rounded-xl bg-muted" />
      <div className="grid grid-cols-1 self-stretch md:grid-cols-2">
        <div className="m-3 h-56 rounded-xl bg-muted" />
        <div className="m-3 h-56 rounded-xl bg-muted" />
      </div>
    </div>
  );
}
