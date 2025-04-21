import { Component, EventEmitter, Input, Output, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ReactiveFormsModule, FormGroup, FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-modal-form',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule
  ],
  templateUrl: './modal-form.component.html',
  styleUrls: ['./modal-form.component.css']
})
export class ModalFormComponent {
  form: FormGroup;
  title: string = '';
  fields: { name: string; label: string; type: string }[] = [];

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ModalFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.title = data.title;
    this.fields = data.fields;
    this.form = this.fb.group({});
  }

  ngOnInit() {
    // Crear controles del formulario basados en los campos proporcionados
    this.fields.forEach(field => {
      this.form.addControl(field.name, this.fb.control(this.data.initialData?.[field.name] || ''));
    });
  }

  onSubmit() {
    if (this.form.valid) {
      const formValue = this.form.value;
      const changes: any = {};

      // Solo incluir campos que han sido modificados
      Object.keys(formValue).forEach(key => {
        if (formValue[key] !== this.data.initialData?.[key]) {
          changes[key] = formValue[key];
        }
      });

      this.dialogRef.close(changes);
    }
  }

  onCancel() {
    this.dialogRef.close();
  }
}
