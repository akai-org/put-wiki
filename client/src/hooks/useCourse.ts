import { useQuery } from '@tanstack/react-query';
import { CourseSchema, type Course } from '@/schemas/course';
import { agent } from '@/lib/api';

async function fetchCourse(slug: string): Promise<Course> {
  const response = await agent.get(`/mocks/${slug}.json`);
  return CourseSchema.parse(response.data);
}

export function useCourse(slug: string) {
  return useQuery<Course>({
    queryKey: ['course', slug],
    queryFn: () => fetchCourse(slug),
    staleTime: 1000 * 60 * 5, // 5 minutes
  });
}
