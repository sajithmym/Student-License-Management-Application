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
  countries = ['Country1', 'Country2', 'Country3']; // Static country options
  institutes = ['Institute1', 'Institute2', 'Institute3']; // Static institute options
  fileTooLarge = false; // File size validation flag
  invalidFileType = false; // File type validation flag
  filename: string | null = "No File Selected"; // Display selected file name
  studentIdCard: File | null = null; // Store the selected file

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
    const allowedFileTypes = ['image/jpeg', 'image/png', 'image/jpg']; // File type check

    // Validate file size and type
    if (file && file.size > 10 * 1024 * 1024) { // Limit file size to 10MB
      this.fileTooLarge = true;
      this.invalidFileType = false;
      this.studentIdCard = null;
      this.filename = "No File Selected";
    } else if (file && !allowedFileTypes.includes(file.type)) { // Check file type
      this.invalidFileType = true;
      this.fileTooLarge = false;
      this.studentIdCard = null;
      this.filename = "No File Selected";
    } else {
      this.fileTooLarge = false;
      this.invalidFileType = false;
      this.studentIdCard = file;
      this.filename = file.name;
    }
  }

  onSubmit() {
    if (this.studentLicenseForm.valid && this.studentIdCard) {
      const formData = new FormData();
      console.log(this.studentLicenseForm.value);
      // Append all form fields
      Object.keys(this.studentLicenseForm.value).forEach(key => {
        formData.append(key, this.studentLicenseForm.value[key]);
      });
      // Append the file to FormData
      formData.append('file', this.studentIdCard, this.studentIdCard.name);

      // Post the data to the backend API
      this.http.post(`${values.backend_address}/api/studentlicense`, formData)
        .subscribe({
          next: (response) => {
            alert('Form submitted successfully!');
            this.studentLicenseForm.reset();
            this.filename = "No File Selected";
            this.studentIdCard = null;
          },
          error: (error) => {
            const errorMessage = error.error ? error.error : 'An unexpected error occurred.';
            console.log(errorMessage);

          }
        });
    } else if (!this.studentIdCard) {
      alert('Please select a file before submitting.');
    }
  }
}
