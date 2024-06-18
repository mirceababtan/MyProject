import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../services/course.service';
import { SnackbarService } from '../services/snackbar.service';
import { AuthService } from '../services/auth.service';
import { Title } from '@angular/platform-browser';

@Component({
  selector: 'app-course',
  templateUrl: './course.component.html',
  styleUrl: './course.component.scss',
})
export class CourseComponent implements OnInit {
  isLoggedIn: any;
  course: any;
  lessonsPreview: any;
  isEnrolled: boolean = false;

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
    this.checkLoggedUser();
  }

  checkLoggedUser() {
    let token = this.authService.getToken();
    if (token) {
      this.isLoggedIn = true;
    }
  }

  loadCourse(courseId: string | null) {
    this.courseService.getCourseById(courseId).subscribe((course) => {
      this.course = course;
      this.titleService.setTitle(this.course.title);
      this.checkEnrollment(courseId);
      this.loadLessonsPreview(courseId);
    });
  }

  loadLessonsPreview(courseId: string | null) {
    this.courseService.getLessonPreviews(courseId).subscribe((lessons) => {
      this.lessonsPreview = lessons.sort((a: any, b: any) => {
        return +a.lessonNumber - +b.lessonNumber;
      });
    });
  }

  checkEnrollment(courseId: string | null) {
    if (!this.isLoggedIn) return;
    this.courseService.isUserEnrolled(courseId).subscribe((response) => {
      this.isEnrolled = response.isEnrolled;
    });
  }

  onLessonClick(lessonId: string) {
    if (this.isLoggedIn) {
      this.router.navigate(['/courses', this.course.id, 'lessons', lessonId]);
    } else {
      this.snackBarService.showLoginRequiredMessage();
    }
  }

  toggleEnrollment() {
    if (this.isEnrolled) {
      this.courseService.unenrollUserFromCourse(this.course.id).subscribe();
    } else {
      this.courseService.enrollUserToCourse(this.course.id).subscribe();
    }
    this.isEnrolled = !this.isEnrolled;
  }
}
