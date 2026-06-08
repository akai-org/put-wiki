import { z } from 'zod';

// There is a few language options returned from USOS API (we will probably change it later) additionally in usos API its called LangDict
export const LocalizedStringSchema = z.object({
  pl: z.string(),
  en: z.string(),
});
