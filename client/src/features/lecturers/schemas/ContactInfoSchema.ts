import { z } from 'zod';

export const ContactInfoSchema = z.object({
  email: z.email(),
  phone: z.string(),
  websiteUrl: z.url(),
});

export type ContactInfoType = z.infer<typeof ContactInfoSchema>;
