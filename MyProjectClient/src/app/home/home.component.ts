import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { CourseService } from '../services/course.service';
import { Course } from '../models/course';
import { SnackbarService } from '../services/snackbar.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent implements OnInit {
  buttonClicked: boolean = false;
  randomCourses: Course[] = [];

  constructor(
    private snackbarService: SnackbarService,
    private courseService: CourseService,
    private router: Router
  ) {}
  ngOnInit(): void {
    this.courseService.getCourses().subscribe((courses: Course[]) => {
      courses = this.shuffleArray(courses);

      this.randomCourses = courses.slice(0, 3);
    });
  }

  showSnackbar() {
    this.snackbarService.showInfoSnackbar(
      'You must login or register to see details about this course.'
    );
  }

  shuffleArray(array: Course[]): Course[] {
    for (let i = array.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
  }

  viewCourse(id: string) {
    this.router.navigate(['/courses', id]);
  }
}
