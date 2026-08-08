import { z } from 'zod';

export const ContactInfoSchema = z.object({ email: z.email(), phone: z.string() });

export type ContactInfo = z.infer<typeof ContactInfoSchema>;
