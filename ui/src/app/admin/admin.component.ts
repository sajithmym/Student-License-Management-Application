import { Component, OnInit } from '@angular/core';
import { AuthService } from '../Service/AllHttpRequests';
import { HttpResponse } from '@angular/common/http';
import { values } from 'constant';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {
  Applications: any[] = [];
  filteredApplications: any[] = [];
  showPopup: boolean = false;
  loading: boolean = true;
  countries = values.countries;
  institutes = values.institutes;
  searchTerm: string = '';
  selectedCountry: string = '';
  selectedInstitute: string = '';

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
            this.loading = false;
          }
        },
        (error: any) => {
          console.error('Error validating token', error);
          this.showPopup = true;
          this.loading = false;
        }
      );
    } else {
      this.showPopup = true;
      this.loading = false;
    }
  }

  loadApplications() {
    this.authService.getApplications().subscribe(
      (data: any[]) => {
        this.Applications = data.map(application => ({ ...application, editing: false }));
        this.applyFilters();
        this.loading = false;
      },
      (error: any) => {
        console.error('Error fetching applications', error);
        this.loading = false;
      }
    );
  }

  applyFilters() {
    this.filteredApplications = this.Applications.filter(application => {
      const matchesSearchTerm = application.name.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        application.email.toLowerCase().includes(this.searchTerm.toLowerCase());
      const matchesCountry = this.selectedCountry ? application.country === this.selectedCountry : true;
      const matchesInstitute = this.selectedInstitute ? application.institute === this.selectedInstitute : true;
      return matchesSearchTerm && matchesCountry && matchesInstitute;
    });
  }

  editCourse(application: any) {
    application.editing = true;
  }

  saveCourse(application: any) {
    application.editing = false;
    this.authService.editApplication(application.id, application).subscribe(
      () => {
        alert('Course Updated successfully');
      },
      (error: any) => {
        console.error('Error saving course', error);
      }
    );
  }

  cancelEdit(application: any) {
    application.editing = false;
    this.loadApplications();
  }

  deleteCourse(application: any) {
    if (confirm('Are you sure you want to delete this course?')) {
      this.authService.deleteApplication(application.id).subscribe(
        () => {
          this.loadApplications();
        },
        (error: any) => {
          console.error('Error deleting course', error);
        }
      );
    }
  }

  viewPicture(event: Event, application: any) {
    event.preventDefault();
    this.authService.getPicture(application.id).subscribe(
      (response: HttpResponse<Blob>) => {
        if (response && response.body && response.headers) {
          const fileExtension = response.headers.get('File-Extension') || '.png';
          const contentType = response.headers.get('Content-Type') || 'application/octet-stream';
          const blob = new Blob([response.body], { type: contentType });
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          const fileName = application.name + fileExtension;
          a.href = url;
          a.download = fileName;
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
          window.URL.revokeObjectURL(url);
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