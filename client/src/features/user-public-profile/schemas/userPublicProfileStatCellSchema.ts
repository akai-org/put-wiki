import type { ReactNode } from 'react';
import { z } from 'zod';

const UserPublicProfileStatCellPropsSchema = z.object({
  className: z.string(),
  label: z.custom<ReactNode>(),
  value: z.number().optional(),
  valueClassName: z.string(),
});
type UserPublicProfileStatCellProps = z.infer<typeof UserPublicProfileStatCellPropsSchema>;

export { UserPublicProfileStatCellPropsSchema };
export type { UserPublicProfileStatCellProps };
