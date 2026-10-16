import { http, HttpResponse } from 'msw';

const coursesMock = {
  name: {
    pl: 'Skibidi sigma',
    en: 'Skibidi sigma',
  },
  description: {
    pl: 'Opis kursu',
    en: 'Course description',
  },
  ectsPoints: 67,
  homepageUrl: 'https://knowyourmeme.com/memes/subcultures/skibidi-toilet',
  bibliography: {
    pl: 'Bibliografia kursu',
    en: 'Course bibliography',
  },
  learningOutcomes: {
    pl: 'Efekty uczenia się',
    en: 'Learning outcomes',
  },
  assessmentCriteria: {
    pl: 'Musisz zrobić 67',
    en: 'You must complete 67',
  },
  facultyId: '2137',

  slug: 'skibidi-sigma',
  lecturers: ['Jan Kowalski', 'Jan Nowak'],
  ratings: [4, 5, 3],
  reviews: ['Świetny kurs!', 'Bardzo pomocny'],
  materials: [
    {
      label: 'Materiał 1',
      url: 'https://knowyourmeme.com/memes/subcultures/skibidi-toilet',
    },
    {
      label: 'Materiał 2',
      url: 'https://knowyourmeme.com/memes/subcultures/skibidi-toilet',
    },
  ],
};

export const coursesHandlers = [
  http.get('/courses/:slug', ({ params }) => {
    const slug = params.slug;

    if (slug !== 'skibidi-sigma') {
      return HttpResponse.json({ message: 'Not found' }, { status: 404 });
    }

    return HttpResponse.json(coursesMock);
  }),
];
