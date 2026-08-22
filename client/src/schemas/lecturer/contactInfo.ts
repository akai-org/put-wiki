import { z } from 'zod';

export const ContactInfoSchema = z.object({
  email: z.email(),
  phone: z.string(),
  website: z.url(),
});

export type ContactInfo = z.infer<typeof ContactInfoSchema>;
