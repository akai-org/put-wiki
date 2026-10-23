import { z } from 'zod';
import { UserPublicSchema } from './userPublicProfileSchema';

const UserPublicProfileStatusPropsSchema = UserPublicSchema.pick({
  joinedDate: true,
  userStatus: true,
}).partial();
type UserPublicProfileStatusProps = z.infer<typeof UserPublicProfileStatusPropsSchema>;

export { UserPublicProfileStatusPropsSchema };
export type { UserPublicProfileStatusProps };
