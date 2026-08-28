export const LecturerQueryKeys = {
  all: ['lecturers'] as const,
  bySlug: (slug: string) => [...LecturerQueryKeys.all, slug] as const,
};
