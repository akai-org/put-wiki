import { z } from 'zod';
import { LocalizedStringSchema } from '@/schemas/common'; // Language options Schema

const CourseSchema = z.object({
  // ------------- Directly from USOS API -------------

  name: LocalizedStringSchema,
  description: LocalizedStringSchema,
  ectsPoints: z.number().nullable(), //ects_credits_simplified in USOS API
  homepageUrl: z.url().nullable(),
  bibliography: LocalizedStringSchema, //List of recommended books and materials for the course
  learningOutcomes: LocalizedStringSchema, //What students should know after completing the course
  assessmentCriteria: LocalizedStringSchema, //Grading criteria for example written exam (60%) and programming project (40%)
  facultyId: z.string(), // for example 06 for Wydział Informatyki

  // ------------- Specific for PUT-WIKI(needs to be done on our backend) -------------
  slug: z.string(), // for example: "analiza-i-struktury-danych"
  lecturers: z.array(z.string()),
  ratings: z.array(z.number().min(1).max(5)),
  reviews: z.array(z.string()), //List of student reviews for the course
  materials: z.array(
    z.object({
      label: z.string(),
      url: z.url(),
    })
  ),
});

type Course = z.infer<typeof CourseSchema>;

export type { Course };
export { CourseSchema };
