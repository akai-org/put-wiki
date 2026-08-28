export const degreeCourseKeys = {
  all: ['degreeCourses'] as const,
  bySlug: (slug: string) => [...degreeCourseKeys.all, slug] as const,
};
