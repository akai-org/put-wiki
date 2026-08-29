import { http, HttpResponse } from 'msw';

const lecturerMock = {
  id: 67,
  slug: 'jan-kowalski',
  baseInfo: {
    name: 'Jan Kowalski',
    title: 'dr inż.',
    photoUrl: 'https://placehold.co/300x300',
  },
  contactInfo: {
    email: 'jan.kowalski@example.com',
    phone: '+48 600 123 456',
    websiteUrl: 'https://github.com/akai-org/put-wiki/issues',
  },
  description:
    'Doktor inżynier specjalizujący się w programowaniu, projektowaniu systemów informatycznych oraz tworzeniu i optymalizacji baz danych. W swojej pracy łączy wiedzę teoretyczną z praktycznym doświadczeniem, zwracając szczególną uwagę na jakość, bezpieczeństwo i skalowalność tworzonych rozwiązań. Prowadzi zajęcia obejmujące zarówno podstawy informatyki, jak i zaawansowane zagadnienia związane z tworzeniem oprogramowania. Chętnie wspiera studentów w realizacji projektów, rozwijaniu umiejętności analitycznego myślenia oraz poszukiwaniu skutecznych rozwiązań problemów technicznych.',
};

export const handlers = [
  http.get('https://api.example.com/user', () => {
    return HttpResponse.json({
      id: 'abc-123',
      firstName: 'John',
      lastName: 'Maverick',
    });
  }),
  
  http.get('/mocks/:slug.json', ({ params }) => {
    const slug = params.slug;

    if (slug !== 'jan-kowalski') {
      return HttpResponse.json({ message: 'Not found' }, { status: 404 });
    }

    return HttpResponse.json(lecturerMock);
  }),
];
