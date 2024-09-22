export interface Course {
  id: string;
  title: string;
  description: string;
  instructorId: string;
  instructor: any;
  lessonCount: number;
  imageUrl: string;
  createdAt: Date;
  isCompleted?: boolean;
}
