import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-student-license-form',
  templateUrl: './license-application.component.html',
  styleUrls: ['./license-application.component.css']
})
export class StudentLicenseFormComponent {
  studentLicenseForm: FormGroup;
  countries = ['Country1', 'Country2', 'Country3'];
  institutes = ['Institute1', 'Institute2', 'Institute3'];
  fileTooLarge = false;

  constructor(private fb: FormBuilder) {
    this.studentLicenseForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      studentEmail: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      address: [''],
      country: ['', Validators.required],
      institute: ['', Validators.required],
      intake: ['', Validators.required],
      courseTitle: ['', Validators.required],
      studentIdCard: [null, Validators.required]
    });
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file && file.size > 10 * 1024 * 1024) {
      this.fileTooLarge = true;
      this.studentLicenseForm.patchValue({ studentIdCard: null });
    } else {
      this.fileTooLarge = false;
      this.studentLicenseForm.patchValue({ studentIdCard: file });
    }
  }

  onSubmit() {
    if (this.studentLicenseForm.valid) {
      const formData = new FormData();
      for (const key in this.studentLicenseForm.value) {
        formData.append(key, this.studentLicenseForm.value[key]);
      }

      // Make HTTP request to backend API
      // Example: this.http.post('api/student-license', formData).subscribe();

      alert('Form submitted successfully!');
      this.studentLicenseForm.reset();
    }
  }
}
