import { z } from 'zod';
import { BaseInfoSchema } from './BaseInfoSchema';
import { ContactInfoSchema } from './ContactInfoSchema';

export const LecturerSchema = z.object({
  id: z.int(),
  slug: z.string(),
  baseInfo: BaseInfoSchema,
  contactInfo: ContactInfoSchema,
  description: z.string(),
});

export type LecturerType = z.infer<typeof LecturerSchema>;
