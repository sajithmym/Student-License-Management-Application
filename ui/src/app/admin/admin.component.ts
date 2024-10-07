import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Service/AllHttpRequests'; // Import the AuthService
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  Applications: any[] = [];
  showPopup: boolean = false;
  editingCourse: any = null; // Track the course being edited
  loading: boolean = true; // Track loading state

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.checkAuthentication();
  }

  /**
   * Checks if the user is authenticated by validating the token.
   * If valid, loads the applications; otherwise, shows the login popup.
   */
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
            this.loading = false; // Stop loading if authentication fails
          }
        },
        (error: any) => {
          console.error('Error validating token', error);
          this.showPopup = true;
          this.loading = false; // Stop loading if authentication fails
        }
      );
    } else {
      this.showPopup = true;
      this.loading = false; // Stop loading if no token is found
    }
  }

  /**
   * Loads the list of applications from the server.
   */
  loadApplications() {
    this.authService.getApplications().subscribe(
      (data: any[]) => {
        this.Applications = data;
        this.loading = false; // Stop loading when data is fetched
      },
      (error: any) => {
        console.error('Error fetching applications', error);
        this.loading = false; // Stop loading if there's an error
      }
    );
  }

  /**
   * Initiates the editing mode for a specific course.
   * @param course The course to be edited.
   */
  editCourse(course: any) {
    this.editingCourse = { ...course }; // Clone the course to avoid direct mutation
  }

  /**
   * Saves the edited course by sending the updated data to the server.
   */
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

  /**
   * Cancels the editing mode without saving changes.
   */
  cancelEdit() {
    this.editingCourse = null; // Exit edit mode without saving
  }

  /**
   * Deletes a specific course after user confirmation.
   * @param course The course to be deleted.
   */
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

  /**
   * Fetches and downloads the picture associated with a specific course.
   * @param event The event triggered by the user action.
   * @param course The course whose picture is to be viewed.
   */
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

  /**
   * Closes the login popup.
   */
  closePopup() {
    this.showPopup = false;
  }

  /**
   * Submits the password for authentication and retrieves a token if successful.
   * @param password The password entered by the user.
   */
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