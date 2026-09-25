import { queryOptions } from '@tanstack/react-query';
import { api } from '@/lib/api';

import { lecturerSchema, type Lecturer } from '@/features/lecturers/schemas/lecturerSchema';
import { lecturerQueryKeys } from './queryKeys';

async function getLecturer(slug: string): Promise<Lecturer> {
  const response = await api.get(`/mocks/${slug}.json`);
  return lecturerSchema.parse(response.data);
}

const lecturerQueries = {
  bySlug: (slug: string) =>
    queryOptions({
      queryKey: lecturerQueryKeys.bySlug(slug),
      queryFn: () => getLecturer(slug),
      staleTime: 1000 * 60 * 5, // 5 minutes
    }),
};

export { lecturerQueries };
