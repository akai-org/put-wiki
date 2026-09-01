import { z } from 'zod';

const UserPublicSchema = z.object({
  nickname: z.string(),
  avatar: z.string().optional(),
  opinions: z.number(),
  reactions: z.number(),
  karma: z.number(),
});
export type UserPublic = z.infer<typeof UserPublicSchema>;
export { UserPublicSchema };
