import { agent } from '@/lib/api';
import { LecturerSchema, type Lecturer } from '@/schemas/lecturer/lecturer';
import { useQuery } from '@tanstack/react-query';

async function fetchLecturer(slug: string): Promise<Lecturer> {
  const response = await agent.get(`/mocks/${slug}.json`);
  return LecturerSchema.parse(response.data);
}

export function useLecturer(slug: string) {
  return useQuery<Lecturer>({
    queryKey: ['lecturer', slug],
    queryFn: () => fetchLecturer(slug),
    staleTime: 1000 * 60 * 30,
  });
}
