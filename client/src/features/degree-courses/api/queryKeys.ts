export const degreeCourseKeys = {
  all: ['degreeCourses'] as const,
  detail: (slug: string) => [...degreeCourseKeys.all, slug] as const,
};
