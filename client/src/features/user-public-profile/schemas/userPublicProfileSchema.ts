import { z } from 'zod';

const UserPublicSchema = z.object({
  nickname: z.string(),
  joinedDate: z.iso.date(),
  userStatus: z.enum(['Active', 'Inactive']),
  opinions: z.number(),
  reactions: z.number(),
  karma: z.number(),
});
export type UserPublic = z.infer<typeof UserPublicSchema>;
export { UserPublicSchema };
