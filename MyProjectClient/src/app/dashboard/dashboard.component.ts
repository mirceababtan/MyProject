import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { CourseService } from '../services/course.service';
import { Course } from '../models/course';
import { Router } from '@angular/router';
import { Observable, forkJoin } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  user: any;
  enrolledCourses: Course[] = [];
  allCourses: Course[] = [];
  displayedCourses: Course[] = [];
  showAllCourses: boolean = false;
  searchText: string = '';
  showCompletedCourses: boolean = false;

  constructor(
    private authService: AuthService,
    private courseService: CourseService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const userDetails = this.authService.getUserDetailsFromToken();
    if (userDetails) this.user = userDetails;

    this.loadEnrolledCourses();
  }

  loadEnrolledCourses() {
    this.courseService
      .getUsersEnrolledCourses(this.user.id)
      .pipe(
        switchMap((courses) => {
          const courseObservables = courses.map((course) =>
            this.isCourseCompleted(course)
          );
          return forkJoin(courseObservables);
        })
      )
      .subscribe((completedCourses) => {
        this.enrolledCourses = completedCourses;
        this.updateDisplayedCourses();
      });
  }

  isCourseCompleted(course: Course): Observable<Course> {
    return this.courseService.isCourseCompleted(course.id).pipe(
      map((response: any) => {
        course.isCompleted = response.isCourseCompleted;
        return course;
      })
    );
  }

  loadAllCourses() {
    this.courseService
      .getCourses()
      .pipe(
        switchMap((courses) => {
          const courseObservables = courses.map((course) =>
            this.isCourseCompleted(course)
          );
          return forkJoin(courseObservables);
        })
      )
      .subscribe((completedCourses) => {
        this.allCourses = completedCourses;
        this.updateDisplayedCourses();
      });
  }

  toggleCoursesView() {
    this.showAllCourses = !this.showAllCourses;
    if (this.showAllCourses) {
      this.loadAllCourses();
    } else {
      this.loadEnrolledCourses();
    }
  }

  toggleCompletedCourses() {
    this.updateDisplayedCourses();
  }

  updateDisplayedCourses() {
    if (this.showAllCourses) {
      this.displayedCourses = this.allCourses;
    } else {
      this.displayedCourses = this.enrolledCourses;
    }

    if (!this.showCompletedCourses) {
      this.displayedCourses = this.displayedCourses.filter(
        (course) => !course.isCompleted
      );
    }

    if (this.searchText.trim() !== '') {
      this.searchCourses();
    }
  }

  searchCourses() {
    this.displayedCourses = this.displayedCourses.filter((course) =>
      course.title.toLowerCase().includes(this.searchText.toLowerCase())
    );
  }

  isEnrolled(courseId: string): any {
    return this.enrolledCourses.some((course) => course.id === courseId);
  }

  viewCourse(courseId: string) {
    this.router.navigate(['/courses', courseId]);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
  }

  navigateToProfile() {
    this.router.navigate(['/profile-page']);
  }

  navigateToAddCourse() {
    this.router.navigate(['/add-course']);
  }
  isInstructor(): boolean {
    if (this.user.role === 'Admin' || this.user.role === 'Instructor')
      return true;
    return false;
  }

  isAdmin(): boolean {
    if (this.user.role === 'Admin') return true;
    return false;
  }
}
