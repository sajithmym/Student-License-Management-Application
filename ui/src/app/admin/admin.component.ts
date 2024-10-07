import { Component, OnInit } from '@angular/core';
import { AuthService } from './service';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  courses: any[] = [];
  showPopup: boolean = false;
  editingCourse: any = null; // Track the course being edited

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.checkAuthentication();
  }

  checkAuthentication() {
    const token = localStorage.getItem('token');
    if (token) {
      this.authService.validateToken(token).subscribe(
        (response: any) => {
          if (response.valid) {
            this.showPopup = false;
            this.loadApplications();
          } else {
            this.showPopup = true;
          }
        },
        (error: any) => {
          console.error('Error validating token', error);
          this.showPopup = true;
        }
      );
    } else {
      this.showPopup = true;
    }
  }

  loadApplications() {
    this.authService.getApplications().subscribe(
      (data: any[]) => {
        this.courses = data;
      },
      (error: any) => {
        console.error('Error fetching applications', error);
      }
    );
  }

  editCourse(course: any) {
    this.editingCourse = { ...course }; // Clone the course to avoid direct mutation
  }

  saveCourse() {
    if (this.editingCourse) {
      this.authService.editApplication(this.editingCourse.id, this.editingCourse).subscribe(
        () => {
          this.loadApplications();
          this.editingCourse = null; // Exit edit mode
        },
        (error: any) => {
          console.error('Error saving course', error);
        }
      );
    }
  }

  cancelEdit() {
    this.editingCourse = null; // Exit edit mode without saving
  }

  deleteCourse(course: any) {
    if (confirm('Are you sure you want to delete this course?')) {
      this.authService.deleteApplication(course.id).subscribe(
        () => {
          this.loadApplications();
        },
        (error: any) => {
          console.error('Error deleting course', error);
        }
      );
    }
  }

  viewPicture(event: Event, course: any) {
    event.preventDefault(); // Prevent the default action of the anchor tag
    this.authService.getPicture(course.id).subscribe(
      (response: HttpResponse<Blob>) => {
        if (response && response.body && response.headers) {
          const fileExtension = response.headers.get('File-Extension') || '.png';
          const contentType = response.headers.get('Content-Type') || 'application/octet-stream';
          const blob = new Blob([response.body], { type: contentType });
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          const fileName = course.name + fileExtension;
          a.href = url;
          a.download = fileName; // Extract file name from headers
          document.body.appendChild(a); // Append the anchor to the body
          a.click(); // Trigger the download
          document.body.removeChild(a); // Remove the anchor from the body
          window.URL.revokeObjectURL(url); // Clean up the URL object
        } else {
          console.error('Invalid response structure', response);
        }
      },
      (error: any) => {
        console.error('Error fetching picture', error);
      }
    );
  }

  closePopup() {
    this.showPopup = false;
  }

  submitPassword(password: string) {
    this.authService.login(password).subscribe(
      (response: any) => {
        if (response.token) {
          localStorage.setItem('token', response.token);
          this.loadApplications();
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