import { z } from 'zod';

// Mirrors UserProfileDto returned by GET /api/user/profile
const UserSchema = z.object({
  userId: z.string(),
  isAuthenticated: z.boolean(),
  authenticationType: z.string(),
});

type User = z.infer<typeof UserSchema>;

export type { User };
export { UserSchema };
