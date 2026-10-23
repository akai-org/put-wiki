export const lecturerQueryKeys = {
  all: ['lecturers'] as const,
  bySlug: (slug: string) => [...lecturerQueryKeys.all, slug] as const,
};
