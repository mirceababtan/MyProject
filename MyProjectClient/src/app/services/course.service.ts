// src/app/course.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environment/environment';
import { Course } from '../models/course';
import { AuthService } from './auth.service';
import { Lesson } from '../models/lesson';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  constructor(private http: HttpClient, private authService: AuthService) {}

  getCourses(): Observable<Course[]> {
    return this.http.get<Course[]>(`${environment.apiUrl}/Course/PreviewAll`, {
      withCredentials: true,
    });
  }
  getCourseById(id: string | null) {
    let params = new HttpParams();
    if (id) params = params.set('id', id);

    return this.http.get<Course>(`${environment.apiUrl}/Course/GetCourseById`, {
      withCredentials: true,
      params: params,
    });
  }

  getUsersEnrolledCourses(id: string) {
    let params = new HttpParams();
    if (id) params = params.set('id', id);

    return this.http.get<Course[]>(
      `${environment.apiUrl}/Course/GetEnrolledById`,
      { withCredentials: true, params: params }
    );
  }

  getLessonPreviews(id: string | null) {
    let params = new HttpParams();
    if (id) params = params.set('id', id);

    return this.http.get<any>(
      `${environment.apiUrl}/Course/LessonPreviewsAll`,
      { params: params }
    );
  }

  enrollUserToCourse(courseId: any) {
    let { id } = this.authService.getUserDetailsFromToken();

    return this.http.post<any>(
      `${environment.apiUrl}/Course/EnrollUserToCourse`,
      { userId: id, courseId: courseId },
      { withCredentials: true }
    );
  }
  unenrollUserFromCourse(courseId: any) {
    let { id } = this.authService.getUserDetailsFromToken();

    let params = new HttpParams();
    params = params.set('userId', id);
    params = params.set('courseId', courseId);

    return this.http.delete<any>(
      `${environment.apiUrl}/Course/UnenrollUserFromCourse`,
      { params: params, withCredentials: true }
    );
  }

  isUserEnrolled(courseId: string | null) {
    let { id } = this.authService.getUserDetailsFromToken();

    let params = new HttpParams();
    if (courseId) {
      params = params.set('userId', id);
      params = params.set('courseId', courseId);
    }

    return this.http.get<any>(`${environment.apiUrl}/Course/IsUserEnrolled`, {
      params: params,
      withCredentials: true,
    });
  }

  getLesson(lessonId: string | null) {
    let params: HttpParams = new HttpParams();
    if (lessonId) params = params.set('id', lessonId);
    return this.http.get<Lesson>(`${environment.apiUrl}/Course/GetLessonById`, {
      params: params,
      withCredentials: true,
    });
  }
}
