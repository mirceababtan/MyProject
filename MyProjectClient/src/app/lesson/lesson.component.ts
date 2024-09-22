import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../services/course.service';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-lesson',
  templateUrl: './lesson.component.html',
  styleUrls: ['./lesson.component.scss'],
})
export class LessonComponent implements OnInit {
  lessonId: string | null = '';
  lesson: any;
  isCompleted: boolean = false;
  isFirstLesson: boolean = false;
  previousLessonId: string = '';
  nextLessonId: string = '';

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private courseService: CourseService,
    public sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe((params) => {
      this.lessonId = params.get('lessonId');
      this.loadLesson();
      this.isLessonCompleted();
    });
  }

  loadLesson() {
    this.courseService.getLesson(this.lessonId).subscribe((lesson) => {
      this.lesson = lesson;
      if (this.lesson.lessonNumber == 1) this.isFirstLesson = true;

      this.courseService
        .getPreviousLessonId(this.lesson.courseId, this.lesson.id)
        .subscribe((response) => {
          this.previousLessonId = response;
          console.log(response, ' == previours response');
        });

      this.courseService
        .getNextLessonId(this.lesson.courseId, this.lesson.id)
        .subscribe((response) => {
          this.nextLessonId = response;
          console.log(response, ' == next response');
        });
    });
  }

  getEmbedUrl(url: string): SafeResourceUrl {
    const videoId = this.extractVideoId(url);
    const embedUrl = `https://www.youtube.com/embed/${videoId}`;
    return this.sanitizer.bypassSecurityTrustResourceUrl(embedUrl);
  }

  extractVideoId(url: string): string {
    const videoIdMatch = url.match(
      /(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})/
    );
    return videoIdMatch ? videoIdMatch[1] : '';
  }

  isLessonCompleted() {
    this.courseService
      .isLessonCompleted(this.lessonId)
      .subscribe((response) => {
        this.isCompleted = response.isLessonCompleted;
      });
  }

  markAsComplete() {
    this.isCompleted = true;
    this.courseService.markLessonAsComplete(this.lessonId).subscribe();
  }

  nextLesson(): void {
    if (this.nextLessonId != '') {
      this.navigateToLesson(this.nextLessonId);
      console.log('clicked');
    }
  }

  previousLesson() {
    if (this.previousLessonId != '') {
      this.navigateToLesson(this.previousLessonId);
    }
  }
  navigateToLesson(lessonId: string): void {
    const courseId = this.lesson?.courseId;
    if (courseId && lessonId) {
      this.router.navigate(['/courses', courseId, 'lessons', lessonId]);
    }
  }
  backToCourse() {
    this.router.navigate(['/courses', this.lesson.courseId]);
  }
}
