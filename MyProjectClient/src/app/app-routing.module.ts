import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './guards/auth-guard';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { CourseComponent } from './course/course.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LessonComponent } from './lesson/lesson.component';
import { AddCourseComponent } from './add-course/add-course.component';
import { ProfileComponent } from './profile/profile.component';
import { EditCourseComponent } from './edit-course/edit-course.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, title: 'Login' },
  { path: 'register', component: RegisterComponent, title: 'Register' },
  {
    path: 'home',
    component: HomeComponent,
    title: 'Home',
  },
  { path: 'courses/:id', component: CourseComponent },
  {
    path: 'courses/:courseId/lessons/:lessonId',
    component: LessonComponent,
    canActivate: [authGuard],
  },
  {
    path: 'dashboard',
    component: DashboardComponent,
    canActivate: [authGuard],
    title: 'Dashboard',
  },
  {
    path: 'courses/edit/:courseid',
    component: EditCourseComponent,
    canActivate: [authGuard],
    title: 'EditCourse',
  },
  {
    path: 'add-course',
    component: AddCourseComponent,
    canActivate: [authGuard],
  },
  {
    path: 'profile-page',
    component: ProfileComponent,
    canActivate: [authGuard],
  },
  {
    path: '**',
    redirectTo: '/dashboard',
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
