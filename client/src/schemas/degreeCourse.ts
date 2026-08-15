import { z } from 'zod';
import { OpinionSchema } from './opinion';

const DegreeCourseSchema = z
  .object({
    DegreeCourseId: z.string(),
    name: z.string(),
    description: z.string(),
    masterDegree: z.number(),
    semesters: z.array(
      z.object({
        number: z.number(),
        subjects: z.array(z.string()),
      })
    ),
    absolventFuture: z.string(),
    hardestSubjects: z.array(
      z.object({
        name: z.string(),
      })
    ),
    worstSubjects: z.array(
      z.object({
        name: z.string(),
        mark: z.number(),
      })
    ),
    opinions: z.array(OpinionSchema),
  })
  .transform((data) => ({
    ...data,
    opinions: data.opinions.map((opinion) => ({
      opinionId: opinion.opinionId,
      userId: opinion.userId,
      userName: opinion.userName,
      content: opinion.content,
      rating: opinion.rating,
      degreeCourseId: data.DegreeCourseId,
    })),
  }));

type DegreeCourse = z.infer<typeof DegreeCourseSchema>;

export type { DegreeCourse };
export { DegreeCourseSchema };
