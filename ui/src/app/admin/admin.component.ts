import { Component } from '@angular/core';

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

  editCourse(course: any) {
    console.log('Edit course', course);
  }
}
