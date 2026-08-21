import { useQuery } from '@tanstack/react-query';
import { DegreeCourseSchema, type DegreeCourse } from '@/schemas/degreeCourse';
import { api } from '@/lib/api';

async function fetchDegreeCourse(slug: string): Promise<DegreeCourse> {
  const response = await api.get(`/mocks/${slug}.json`);
  return DegreeCourseSchema.parse(response.data);
}

export function useDegreeCourse(slug: string) {
  return useQuery<DegreeCourse>({
    queryKey: ['degreeCourse', slug],
    queryFn: () => fetchDegreeCourse(slug),
    staleTime: 1000 * 60 * 5, // 5 minutes
  });
}
