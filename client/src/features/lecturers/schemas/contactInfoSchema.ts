import { z } from 'zod';

export const contactInfoSchema = z.object({
  email: z.email(),
  phone: z.string(),
  websiteUrl: z.url(),
});

export type ContactInfo = z.infer<typeof contactInfoSchema>;
