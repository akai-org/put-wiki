import { z } from 'zod';

export const BaseInfoSchema = z.object({
  name: z.string(),
  title: z.string(),
  photoUrl: z.url(),
});

export type BaseInfoType = z.infer<typeof BaseInfoSchema>;
