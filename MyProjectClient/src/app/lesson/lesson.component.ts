import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../services/course.service';

@Component({
  selector: 'app-lesson',
  templateUrl: './lesson.component.html',
  styleUrls: ['./lesson.component.scss'],
})
export class LessonComponent implements OnInit {
  lessonId: string | null = '';
  lesson: any;

  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService
  ) {}

  ngOnInit(): void {
    this.lessonId = this.route.snapshot.paramMap.get('lessonId');
    this.loadLesson();
  }

  loadLesson() {
    this.courseService.getLesson(this.lessonId).subscribe((lesson) => {
      this.lesson = lesson;
      console.log(lesson);
    });
  }

  markAsComplete() {
    // Implement marking lesson as complete logic
    console.log('Lesson marked as complete');
    // You may want to update lesson status in backend or local storage
  }

  nextLesson() {
    // Implement logic to navigate to the next lesson
    // For simplicity, assume you have a predefined order or structure for lessons
    // Navigate to the next lesson based on your business logic
    console.log('Navigate to next lesson');
    // Example: this.router.navigate(['/courses', this.courseId, 'lessons', nextLessonId]);
  }
}
