import { queryOptions } from '@tanstack/react-query';
import { api } from '@/lib/api';

import { LecturerSchema, type LecturerType } from '@/features/lecturers/schemas/LecturerSchema';
import { LecturerQueryKeys } from './QueryKeys';

async function getLecturer(slug: string): Promise<LecturerType> {
  const response = await api.get(`/mocks/${slug}.json`);
  return LecturerSchema.parse(response.data);
}

const LecturerQueries = {
  bySlug: (slug: string) =>
    queryOptions({
      queryKey: LecturerQueryKeys.bySlug(slug),
      queryFn: () => getLecturer(slug),
      staleTime: 1000 * 60 * 5, // 5 minutes
    }),
};

export { LecturerQueries };
