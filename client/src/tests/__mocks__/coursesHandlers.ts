import { http, HttpResponse } from 'msw';

const coursesMock = {
  // ------------- Directly from USOS API -------------

  name: 'Skibidi sigma',
  description: 'Opis kursu',
  ectsPoints: 67,
  homepageUrl: 'https://knowyourmeme.com/memes/subcultures/skibidi-toilet',
  bibliography: 'https://knowyourmeme.com/memes/subcultures/skibidi-toilet',
  learningOutcomes: 'https://knowyourmeme.com/memes/subcultures/skibidi-toilet',
  assessmentCriteria: 'Musisz zrobić 67',
  facultyId: '2137',

  // ------------- Specific for PUT-WIKI(needs to be done on our backend) -------------
  slug: 'skibidi-sigma', // for example: "analiza-i-struktury-danych"
  lecturers: 'Jan Kowalski, Jan Nowak', // for example: "Jan Kowalski, Jan Nowak"
  ratings: [4, 5, 3],
  reviews: ['Świetny kurs!', 'Bardzo pomocny'], //List of student reviews for the course
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
