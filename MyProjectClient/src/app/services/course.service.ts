// src/app/course.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { environment } from '../../environment/environment';
import { Course } from '../models/course';
import { AuthService } from './auth.service';
import { Lesson } from '../models/lesson';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  constructor(private http: HttpClient, private authService: AuthService) {}

  deleteCourse(id: any) {
    let params = new HttpParams();
    if (id) params = params.set('id', id);
    return this.http.delete(`${environment.apiUrl}/Course/Delete`, {
      params: params,
    });
  }

  updateCourse(courseId: string, value: any) {
    throw new Error('Method not implemented.');
  }

  addCourse(courseData: any) {
    courseData.instructorId = this.authService.getUserDetailsFromToken().id;
    return this.http.post<any>(`${environment.apiUrl}/Course/Add`, courseData, {
      withCredentials: true,
    });
  }

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

  getCompletedLessonsForUserByCourseId(courseId: string): Observable<any[]> {
    let userId = this.authService.getUserDetailsFromToken().id;

    let params = new HttpParams();
    params = params.set('userId', userId);
    params = params.set('courseId', courseId);

    return this.http.get<any[]>(
      `${environment.apiUrl}/Course/GetCompletedLessonForUserByCourseId`,
      { params: params, withCredentials: true }
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

  isCourseCompleted(courseId: string): any {
    let userId = this.authService.getUserDetailsFromToken().id;
    let params = new HttpParams();
    params = params.set('userId', userId);
    params = params.set('courseId', courseId);

    return this.http.get(`${environment.apiUrl}/Course/IsCourseCompleted`, {
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

  getLessonsByCourseId(courseId: string | null) {
    let params = new HttpParams();
    if (courseId) params = params.set('id', courseId);

    return this.http.get<any>(
      `${environment.apiUrl}/Course/GetLessonsByCourseId`,
      { params: params, withCredentials: true }
    );
  }

  getPreviousLessonId(courseId: string, lessonId: string): Observable<any> {
    return this.getLessonsByCourseId(courseId).pipe(
      map((response) => {
        const lessons = response.sort((a: any, b: any) => {
          return +a.lessonNumber - +b.lessonNumber;
        });
        const currentLessonIndex = lessons.findIndex(
          (lesson: any) => lesson.id === lessonId
        );
        if (currentLessonIndex > 0) {
          return lessons[currentLessonIndex - 1].id;
        } else {
          return '';
        }
      })
    );
  }

  getNextLessonId(courseId: string, lessonId: string): Observable<any> {
    return this.getLessonsByCourseId(courseId).pipe(
      map((response) => {
        const lessons = response.sort((a: any, b: any) => {
          return +a.lessonNumber - +b.lessonNumber;
        });
        const currentLessonIndex = lessons.findIndex(
          (lesson: any) => lesson.id === lessonId
        );
        if (currentLessonIndex < lessons.length - 1) {
          return lessons[currentLessonIndex + 1].id;
        } else {
          return '';
        }
      })
    );
  }

  markLessonAsComplete(lessonId: string | null) {
    let userId = this.authService.getUserDetailsFromToken().id;

    return this.http.post(
      `${environment.apiUrl}/Course/MarkLessonAsCompleted`,
      { userId: userId, lessonId: lessonId },
      { withCredentials: true }
    );
  }

  isLessonCompleted(lessonId: string | null) {
    let userId = this.authService.getUserDetailsFromToken().id;
    let params: HttpParams = new HttpParams();
    if (lessonId) {
      params = params.set('userId', userId);
      params = params.set('lessonId', lessonId);
    }
    return this.http.get<any>(
      `${environment.apiUrl}/Course/IsLessonCompleted`,
      {
        params: params,
        withCredentials: true,
      }
    );
  }
}
