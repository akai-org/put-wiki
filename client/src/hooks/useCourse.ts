import { useQuery } from '@tanstack/react-query';
import axios from 'axios';
import { CourseSchema, type Course } from '@/types/course';

export default function useCourse(slug: string) {
  return useQuery<Course>({
    queryKey: ['course', slug],
    queryFn: async () => {
      const response = await axios.get('/mocks/course.json');
      return CourseSchema.parse(response.data);
    },
  });
}
