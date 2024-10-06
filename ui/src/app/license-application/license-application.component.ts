import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { values } from 'constant';

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
  invalidFileType = false;
  filename: string | null = "No File Selected";
  studentIdCard: File | null = null;

  constructor(private fb: FormBuilder, private http: HttpClient) {
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
    });
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    const allowedFileTypes = ['image/jpeg', 'image/png', 'image/jpg'];

    if (file) {
      if (file.size > 10 * 1024 * 1024) {
        this.setFileError(true, false, null, "No File Selected");
      } else if (!allowedFileTypes.includes(file.type)) {
        this.setFileError(false, true, null, "No File Selected");
      } else {
        this.setFileError(false, false, file, file.name);
      }
    }
  }

  setFileError(fileTooLarge: boolean, invalidFileType: boolean, studentIdCard: File | null, filename: string | null) {
    this.fileTooLarge = fileTooLarge;
    this.invalidFileType = invalidFileType;
    this.studentIdCard = studentIdCard;
    this.filename = filename;
  }

  onSubmit() {
    if (this.studentLicenseForm.valid && this.studentIdCard) {
      const formData = new FormData();

      const studentData = {
        name: `${this.studentLicenseForm.value.firstName} ${this.studentLicenseForm.value.lastName}`,
        email: this.studentLicenseForm.value.studentEmail,
        phone: this.studentLicenseForm.value.phone,
        address: this.studentLicenseForm.value.address,
        country: this.studentLicenseForm.value.country,
        institute: this.studentLicenseForm.value.institute,
        courseTitle: this.studentLicenseForm.value.courseTitle,
        intake: this.studentLicenseForm.value.intake,
        licenceStatus: false,
        approvalStatus: false,
        licenceExpiryDate: new Date("1999-12-12")
      };

      formData.append('application', JSON.stringify(studentData));
      formData.append('file', this.studentIdCard, this.studentIdCard.name);

      this.http.post(`${values.backend_address}/api/studentlicense`, formData)
        .subscribe({
          next: () => {
            alert('Form submitted successfully!');
            this.resetForm();
          },
          error: (error) => {
            const errorMessage = error.error ? error.error : 'An unexpected error occurred.';
            console.log(errorMessage);
            alert(errorMessage)
          }
        });
    } else if (!this.studentIdCard) {
      alert('Please select a file before submitting.');
    }
  }

  resetForm() {
    this.studentLicenseForm.reset();
    this.filename = "No File Selected";
    this.studentIdCard = null;
  }
}