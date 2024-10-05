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
  countries = ['Country1', 'Country2', 'Country3']; // Country options
  institutes = ['Institute1', 'Institute2', 'Institute3']; // Institute options
  fileTooLarge = false; // Flag for file size validation
  filename: string | null = "No File Selected"; // Initialize filename variable
  studentIdCard: File | null = null; // Separate variable for the file

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
    if (file && file.size > 10 * 1024 * 1024) { // Limit file size to 10MB
      this.fileTooLarge = true; // Set the flag for file too large
      this.studentIdCard = null; // Reset file if too large
      this.filename = "No File Selected"; // Reset filename
    } else {
      this.fileTooLarge = false; // Set the flag to false if file size is valid
      this.studentIdCard = file; // Save the file reference
      this.filename = file.name; // Store the name of the selected file
    }
  }

  onSubmit() {
    if (this.studentLicenseForm.valid && this.studentIdCard) {
      const formData = new FormData();

      // Append all form field values to the FormData
      Object.keys(this.studentLicenseForm.value).forEach(key => {
        formData.append(key, this.studentLicenseForm.value[key]);
      });

      // Append the file to the FormData with the key matching the backend expectation
      formData.append('file', this.studentIdCard, this.studentIdCard.name); // Use the file name

      // Make HTTP request to backend API
      this.http.post(`${values.backend_address}/api/studentlicense`, formData)
        .subscribe({
          next: (response) => {
            alert('Form submitted successfully!'); // Alert on successful submission
            this.studentLicenseForm.reset(); // Reset form fields
            this.filename = "No File Selected"; // Reset filename after submission
            this.studentIdCard = null; // Reset file after submission
          },
          error: (error) => {
            console.error('Error uploading file:', error); // Log error
            const errorMessage = error.error ? error.error : 'An unexpected error occurred.';
            alert('Error uploading file: ' + errorMessage); // Show error message
          }
        });
    } else if (!this.studentIdCard) {
      alert('Please select a file before submitting.'); // Alert if no file is selected
    }
  }
}
