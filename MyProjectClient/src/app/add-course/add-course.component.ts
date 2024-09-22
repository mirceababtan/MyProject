import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { LessonDialogComponent } from '../lesson-dialog/lesson-dialog.component';
import { CourseService } from '../services/course.service';
import { SnackbarService } from '../services/snackbar.service';

@Component({
  selector: 'app-add-course',
  templateUrl: './add-course.component.html',
  styleUrls: ['./add-course.component.scss'],
})
export class AddCourseComponent implements OnInit {
  deleteLesson(_t49: number) {
    throw new Error('Method not implemented.');
  }
  courseForm: any = new FormGroup({});
  imagePreview: string | ArrayBuffer | null = null;

  constructor(
    private fb: FormBuilder,
    private dialog: MatDialog,
    private courseService: CourseService,
    private snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    this.courseForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      videoUrl: [''],
      imageUrl: [''],
      lessons: this.fb.array([]),
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
        console.log(result);
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
      this.courseService
        .addCourse(this.courseForm.value)
        .subscribe((result) => {
          if (result) {
            this.snackbarService.showCourseSuccess();
            this.resetForm();
          }
        });
    }
  }

  resetForm(): void {
    this.courseForm.reset();

    this.imagePreview = null;

    (this.courseForm.get('lessons') as FormArray).clear();

    this.ngOnInit();
  }
}
