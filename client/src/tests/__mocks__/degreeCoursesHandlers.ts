import { http, HttpResponse } from 'msw';

const degreeCoursesMock = {
  DegreeCourseId: '67',
  name: 'Informatyka',
  description:
    'Program studiów informatycznych obejmuje szeroki zakres zagadnień związanych z technologią komputerową, programowaniem, algorytmami, bazami danych, sieciami komputerowymi oraz sztuczną inteligencją. Studenci zdobywają umiejętności projektowania i implementacji systemów informatycznych, analizy danych oraz rozwiązywania problemów technicznych. Program kładzie również duży nacisk na rozwijanie umiejętności praktycznych poprzez projekty i laboratoria.',
  masterDegree: 1,
  semesters: [
    {
      number: 1,
      subjects: ['Programowanie', 'Algorytmy', 'Bazy danych'],
    },
    {
      number: 2,
      subjects: ['Sieci komputerowe', 'Sztuczna inteligencja', 'Systemy operacyjne'],
    },
  ],
  absolventFuture: 'Brak',
  hardestSubjects: [
    {
      name: 'Programowanie',
    },
  ],
  worstSubjects: [
    {
      name: 'Algorytmy',
      mark: 2,
    },
  ],
  opinion: [
    {
      opinionId: '1',
      userId: '1',
      userName: 'Jan Kowalski',
      content: 'Bardzo trudny przedmiot.',
      rating: 2,
    },
  ],
};

export const degreeCoursesHandlers = [
  http.get('/degree-courses/:slug', ({ params }) => {
    const slug = params.slug;

    console.log('Received slug:', slug); // Log the received slug for debugging

    if (slug !== 'Informatyka') {
      return HttpResponse.json({ message: 'Not found' }, { status: 404 });
    }

    return HttpResponse.json(degreeCoursesMock);
  }),
];
