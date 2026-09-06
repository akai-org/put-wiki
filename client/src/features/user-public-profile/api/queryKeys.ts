export const userPublicProfileKeys = {
  all: ['userPublicProfiles'] as const,
  bySlug: (slug: string) => [...userPublicProfileKeys.all, slug] as const,
};
