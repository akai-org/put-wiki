import { z } from 'zod';
import { baseInfoSchema } from './baseInfoSchema';
import { contactInfoSchema } from './contactInfoSchema';

export const lecturerSchema = z.object({
  id: z.int(),
  slug: z.string(),
  baseInfo: baseInfoSchema,
  contactInfo: contactInfoSchema,
  description: z.string(),
});

export type Lecturer = z.infer<typeof lecturerSchema>;
