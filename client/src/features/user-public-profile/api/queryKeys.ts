export const userPublicProfileKeys = {
  all: ['userPublicProfiles'] as const,
  bySlug: (nicknameSlug: string) => [...userPublicProfileKeys.all, nicknameSlug] as const,
};
