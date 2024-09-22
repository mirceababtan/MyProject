import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-lesson-dialog',
  templateUrl: './lesson-dialog.component.html',
  styleUrls: ['./lesson-dialog.component.scss'],
})
export class LessonDialogComponent implements OnInit {
  lessonForm: FormGroup = new FormGroup({});

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<LessonDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {}

  ngOnInit(): void {
    this.lessonForm = this.fb.group({
      lessonNumber: [this.data.lesson.lessonNumber],
      title: [this.data.lesson.title, Validators.required],
      content: [this.data.lesson.content],
      videoUrl: [this.data.lesson.videoUrl],
      fileUrl: [this.data.lesson.fileUrl],
    });
  }

  onSave(): void {
    if (this.lessonForm.valid) {
      this.dialogRef.close(this.lessonForm.value);
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
