import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { CourseService } from '../services/course.service';
import { Course } from '../models/course';
import { Router } from '@angular/router';

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
      .subscribe((courses) => {
        this.enrolledCourses = courses;
        this.displayedCourses = this.enrolledCourses;
      });
  }

  loadAllCourses() {
    this.courseService.getCourses().subscribe((courses) => {
      this.allCourses = courses;
      this.displayedCourses = this.allCourses;
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

  searchCourses() {
    this.displayedCourses = this.showAllCourses
      ? this.allCourses
      : this.enrolledCourses;
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
    throw new Error('Method not implemented.');
  }
}
