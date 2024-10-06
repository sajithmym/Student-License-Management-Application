import { Component } from '@angular/core';
import { AuthService } from './service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {
  courses = [
    {
      intake: '2024 October',
      courseTitle: 'Bachelor of Construction',
      studentIdCardUrl: '#',
      licenceStatus: 'Inactive',
      approvalStatus: 'Pending',
      licenceExpiryDate: 'N/A'
    },
    {
      intake: '2025 January',
      courseTitle: 'New Zealand Diploma in Construction',
      studentIdCardUrl: '#',
      licenceStatus: 'Inactive',
      approvalStatus: 'Pending',
      licenceExpiryDate: 'N/A'
    },
    {
      intake: '2024 October',
      courseTitle: 'Bachelor of Construction',
      studentIdCardUrl: '#',
      licenceStatus: 'Active',
      approvalStatus: 'Approved',
      licenceExpiryDate: '2025-03-25'
    },
    {
      intake: '2025 January',
      courseTitle: 'New Zealand Diploma in Construction',
      studentIdCardUrl: '#',
      licenceStatus: 'Inactive',
      approvalStatus: 'Rejected',
      licenceExpiryDate: 'N/A'
    }
  ];

  showPopup: boolean = true;

  constructor(private authService: AuthService) { }

  editCourse(course: any) {
    console.log('Edit course', course);
  }

  closePopup() {
    this.showPopup = false;
  }

  submitPassword(password: string) {
    this.authService.authenticate(password).subscribe(
      (response: any) => {
        if (response.success) {
          this.showPopup = false;
        } else {
          alert('Incorrect password');
        }
      },
      (error: any) => {
        alert('Error verifying password');
      }
    );
  }
}
