import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ActivatedRoute, Router } from '@angular/router';
import { CourseService } from '../services/course.service';
import { SnackbarService } from '../services/snackbar.service';
import { LessonDialogComponent } from '../lesson-dialog/lesson-dialog.component';

@Component({
  selector: 'app-edit-course',
  templateUrl: './edit-course.component.html',
  styleUrls: ['./edit-course.component.scss'],
})
export class EditCourseComponent implements OnInit {
  deleteLesson(_t58: number) {
    throw new Error('Method not implemented.');
  }
  courseForm: FormGroup = new FormGroup({});
  imagePreview: string | ArrayBuffer | null = null;
  courseId: string = '';

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private courseService: CourseService,
    private snackbarService: SnackbarService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.courseId = this.route.snapshot.paramMap.get('id')!;
    this.initializeForm();
    this.loadCourseData();
  }

  private initializeForm(): void {
    this.courseForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      videoUrl: [''],
      imageUrl: [''],
      lessons: this.fb.array([]),
    });
  }

  private loadCourseData(): void {
    this.courseService.getCourseById(this.courseId).subscribe((course) => {
      this.courseForm.patchValue(course);
      this.imagePreview = course.imageUrl;
      this.courseService
        .getLessonsByCourseId(this.courseId)
        .subscribe((lessons_data) => {
          const lessons = this.courseForm.get('lessons') as FormArray;
          lessons_data.forEach((lesson: any) => {
            lessons.push(this.fb.group(lesson));
          });
        });
    });
  }

  get lessons(): FormArray {
    return this.courseForm.get('lessons') as FormArray;
  }

  addLesson(): void {
    const lessonForm = this.fb.group({
      lessonNumber: [this.lessons.length + 1],
      title: ['', Validators.required],
      content: [''],
      videoUrl: [''],
      fileUrl: [''],
    });
    this.lessons.push(lessonForm);

    this.editLesson(this.lessons.length - 1);
  }

  editLesson(index: number): void {
    const dialogRef = this.dialog.open(LessonDialogComponent, {
      width: '500px',
      data: { lesson: this.lessons.at(index).value },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.lessons.at(index).patchValue(result);
      }
    });
  }

  onImageSelected(event: Event): void {
    const file = (event.target as HTMLInputElement).files![0];

    if (!file) return;

    const maxSize = 2 * 1024 * 1024;
    const allowedFileTypes = ['image/png'];

    if (file.size > maxSize) {
      this.courseForm.get('image')?.setErrors({ invalidFileSize: true });
      return;
    }

    if (!allowedFileTypes.includes(file.type)) {
      this.courseForm.get('image')?.setErrors({ invalidFileType: true });
      return;
    }

    this.courseForm.get('image')?.setErrors(null);

    const reader = new FileReader();
    reader.onload = () => {
      const base64String = reader.result as string;
      this.courseForm.patchValue({ imageUrl: base64String });
      this.imagePreview = base64String;
    };
    reader.readAsDataURL(file);
  }

  drop(event: CdkDragDrop<string[]>): void {
    const lessonsArray = this.courseForm.get('lessons') as FormArray;
    moveItemInArray(
      lessonsArray.controls,
      event.previousIndex,
      event.currentIndex
    );
    lessonsArray.controls.forEach((control: any, index) => {
      control.get('lessonNumber').setValue(index + 1);
    });
  }

  onSubmit(): void {
    if (this.courseForm.valid) {
      // this.courseService
      //   .updateCourse(this.courseId, this.courseForm.value)
      //   .subscribe((result) => {
      //     if (result) {
      //       this.snackbarService.showCourseSuccess();
      //       this.router.navigate(['/courses']);
      //     }
      //   });
    }
  }
}
