import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../services/course.service';
import { SnackbarService } from '../services/snackbar.service';
import { AuthService } from '../services/auth.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-course',
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.scss'],
})
export class CourseComponent implements OnInit {
  isLoggedIn: boolean = false;
  user: any;
  course: any;
  isAdmin: boolean = false;
  lessonsPreview: any;
  isEnrolled: boolean = false;
  completedLessons: Set<string> = new Set();
  isCourseCompleted: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private snackBarService: SnackbarService,
    private authService: AuthService,
    private titleService: Title,
    private router: Router
  ) {}

  ngOnInit() {
    const courseId = this.route.snapshot.paramMap.get('id');
    this.loadCourse(courseId);
    this.user = this.authService.getUserDetailsFromToken();
    this.checkLoggedUser();
  }

  checkLoggedUser(): void {
    this.isLoggedIn = !!this.authService.getToken();
    if (this.user.role === 'Admin' || this.user.role === 'Instructor')
      this.isAdmin = true;
  }

  loadCourse(courseId: string | null) {
    if (!courseId) return;
    this.courseService.getCourseById(courseId).subscribe((course) => {
      this.course = course;
      this.titleService.setTitle(this.course.title);
      this.checkEnrollment(courseId);
      this.loadLessonsPreview(courseId);
    });
  }

  loadLessonsPreview(courseId: string | null) {
    if (!courseId) return;

    this.courseService
      .getLessonPreviews(courseId)
      .subscribe((lessons: any[]) => {
        let previousLessonCompleted = false;

        this.lessonsPreview = lessons
          .map((lesson) => {
            lesson.isCompleted = this.completedLessons.has(lesson.id);
            lesson.isLocked = !this.completedLessons.has(lesson.id);

            if (lesson.isLocked && previousLessonCompleted) {
              this.courseService
                .getPreviousLessonId(this.course.id, lesson.id)
                .subscribe((id: string) => {
                  if (id !== '') lesson.isLocked = false;
                });
            }

            previousLessonCompleted = lesson.isCompleted;

            return lesson;
          })
          .sort((a, b) => +a.lessonNumber - +b.lessonNumber);
        this.checkCourseCompletion();
      });
  }

  checkEnrollment(courseId: string | null) {
    if (!this.isLoggedIn || !courseId) return;
    this.courseService.isUserEnrolled(courseId).subscribe((response) => {
      this.isEnrolled = response.isEnrolled;
      this.loadCompletedLessons(courseId);
    });
  }

  loadCompletedLessons(courseId: string | null): void {
    if (!this.isLoggedIn || !courseId) return;
    this.courseService
      .getCompletedLessonsForUserByCourseId(courseId)
      .subscribe((completedLessons: any[]) => {
        this.completedLessons = new Set(
          completedLessons.map((lesson: any) => lesson.id)
        );
        this.loadLessonsPreview(courseId);
      });
  }

  onLessonClick(lessonId: string) {
    if (this.isEnrolled) {
      this.router.navigate(['/courses', this.course.id, 'lessons', lessonId]);
    } else {
      this.snackBarService.showEnrollRequiredMessage();
    }
  }

  toggleEnrollment() {
    if (this.isEnrolled) {
      this.courseService
        .unenrollUserFromCourse(this.course.id)
        .subscribe(() => {
          this.isEnrolled = false;
          this.completedLessons.clear();
          this.loadLessonsPreview(this.course.id);
        });
    } else {
      this.courseService.enrollUserToCourse(this.course.id).subscribe(() => {
        this.isEnrolled = true;
        this.loadCompletedLessons(this.course.id);
      });
    }
    this.isEnrolled = !this.isEnrolled;
  }

  checkCourseCompletion(): void {
    const allLessonsCompleted = this.lessonsPreview.every((lesson: any) =>
      this.completedLessons.has(lesson.id)
    );

    if (allLessonsCompleted) {
      this.isCourseCompleted = true;
    }
  }

  editCourse() {
    this.router.navigate(['/courses/edit', this.course.id]);
  }

  deleteCourse() {
    if (confirm('Are you sure you want to delete this course?')) {
      this.courseService.deleteCourse(this.course.id).subscribe(() => {
        this.snackBarService.showSuccessMessage('Course deleted successfully');
        this.router.navigate(['/dashboard']);
      });
    }
  }
}
