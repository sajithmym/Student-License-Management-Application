import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AdminComponent } from './admin/admin.component';
import { StudentLicenseFormComponent } from './license-application/license-application.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { AskPasswordComponent } from './ask-password/ask-password.component';

@NgModule({
  declarations: [
    AppComponent,
    AdminComponent,
    StudentLicenseFormComponent,
    NotFoundComponent,
    AskPasswordComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
