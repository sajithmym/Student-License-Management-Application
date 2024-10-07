import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../Service/AllHttpRequest'; // Import the AuthService

@Component({
  selector: 'app-student-license-form',
  templateUrl: './license-application.component.html',
  styleUrls: ['./license-application.component.css']
})
export class StudentLicenseFormComponent {
  studentLicenseForm: FormGroup;
  countries = ['Srilanka', 'Canada', 'United Kingdom', 'Australia', 'Germany', 'France', 'India', 'China', 'Japan', 'Brazil'];
  institutes = ['University of Colombo', 'University of Peradeniya', 'University of Sri Jayewardenepura', 'University of Kelaniya', 'University of Moratuwa', 'University of Ruhuna', 'Eastern University, Sri Lanka', 'South Eastern University of Sri Lanka', 'Rajarata University of Sri Lanka', 'Sabaragamuwa University of Sri Lanka'];
  fileTooLarge = false;
  invalidFileType = false;
  filename: string | null = "No File Selected";
  studentIdCard: File | null = null;

  constructor(private fb: FormBuilder, private authService: AuthService) { // Inject the AuthService
    // Initialize the form with validation rules
    this.studentLicenseForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      studentEmail: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      address: [''],
      country: ['', Validators.required],
      institute: ['', Validators.required],
      intake: ['', Validators.required],
      courseTitle: ['', Validators.required]
    });
  }

  /**
   * Handles file input change event.
   * Validates the file size and type.
   * @param event The file input change event.
   */
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

  /**
   * Sets the file error state.
   * @param fileTooLarge Indicates if the file is too large.
   * @param invalidFileType Indicates if the file type is invalid.
   * @param studentIdCard The selected file.
   * @param filename The name of the selected file.
   */
  setFileError(fileTooLarge: boolean, invalidFileType: boolean, studentIdCard: File | null, filename: string | null) {
    this.fileTooLarge = fileTooLarge;
    this.invalidFileType = invalidFileType;
    this.studentIdCard = studentIdCard;
    this.filename = filename;
  }

  /**
   * Handles form submission.
   * Validates the form and sends the data to the server.
   */
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
      };

      formData.append('application', JSON.stringify(studentData));
      formData.append('file', this.studentIdCard, this.studentIdCard.name);

      this.authService.submitStudentLicenseForm(formData).subscribe({
        next: () => {
          alert('Form submitted successfully!');
          this.resetForm();
        },
        error: (error) => {
          const errorMessage = error.error ? error.error : 'An unexpected error occurred.';
          console.log(errorMessage);
          alert(errorMessage);
        }
      });
    } else if (!this.studentIdCard) {
      alert('Please select a file before submitting.');
    }
  }

  /**
   * Resets the form to its initial state.
   */
  resetForm() {
    this.studentLicenseForm.reset();
    this.filename = "No File Selected";
    this.studentIdCard = null;
  }
}