export const courseKeys = {
  all: ['courses'] as const,
  bySlug: (slug: string) => [...courseKeys.all, slug] as const,
};
