export const degreeCourseKeys = {
  all: ['degreeCourses'] as const,
  details: () => [...degreeCourseKeys.all, 'detail'] as const,
  detail: (slug: string) => [...degreeCourseKeys.details(), slug] as const,
};
