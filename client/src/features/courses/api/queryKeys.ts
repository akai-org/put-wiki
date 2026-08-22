export const courseKeys = {
  all: ['courses'] as const,
  detail: (slug: string) => [...courseKeys.all, slug] as const,
};
