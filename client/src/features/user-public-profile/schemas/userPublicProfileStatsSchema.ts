import { z } from 'zod';
import { UserPublicSchema } from './userPublicProfileSchema';

const UserPublicProfileStatsPropsSchema = UserPublicSchema.pick({
  karma: true,
  opinions: true,
  reactions: true,
}).partial();
type UserPublicProfileStatsProps = z.infer<typeof UserPublicProfileStatsPropsSchema>;

export { UserPublicProfileStatsPropsSchema };
export type { UserPublicProfileStatsProps };
