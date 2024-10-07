import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StudentLicenseFormComponent } from './license-application/license-application.component';
import { AdminComponent } from './admin/admin.component';
import { NotFoundComponent } from './not-found/not-found.component';

const routes: Routes = [
  { path: '', redirectTo: '/student-license-form', pathMatch: 'full' },
  { path: 'student-license-form', component: StudentLicenseFormComponent },
  { path: 'admin', component: AdminComponent },
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
