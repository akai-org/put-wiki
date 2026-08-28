import { z } from 'zod';

const OpinionSchema = z.object({
  opinionId: z.string(),
  degreeCourseId: z.string().optional(),
  courseId: z.string().optional(), //if course is a topic od opinion
  lecturerId: z.string().optional(), // if lecturer is a topic of opinion
  userId: z.string(),
  userName: z.string(),
  content: z.string(),
  rating: z.number().min(1).max(5),
});

type Opinion = z.infer<typeof OpinionSchema>;

export type { Opinion };
export { OpinionSchema };
