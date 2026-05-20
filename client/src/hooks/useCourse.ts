import {useQuery} from "@tanstack/react-query";
import axios from 'axios';
import { CourseSchema,type Course } from "@/types/course";

export default function useCourse(courseId: string) {
    return (
        useQuery<Course>({
            queryKey: ['course', courseId],
            queryFn: async () =>{
                const response = await axios.get('/mocks/course.json');
                return CourseSchema.parse(response.data);
            }
        })
    )
}