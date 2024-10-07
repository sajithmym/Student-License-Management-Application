import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { values } from 'constant';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = values.backend_address + '/api/studentlicense';

    constructor(private http: HttpClient) { }

    authenticate(password: string): Observable<any> {
        return this.http.post<any>(`${values.backend_address}/api/Auth/verify-password`, { password });
    }

    getApplications(): Observable<any[]> {
        return this.http.get<any[]>(this.apiUrl);
    }

    editApplication(id: number, application: any): Observable<any> {
        return this.http.put<any>(`${this.apiUrl}/${id}`, application);
    }

    deleteApplication(id: number): Observable<any> {
        return this.http.delete<any>(`${this.apiUrl}/${id}`);
    }

    getPicture(id: number): Observable<HttpResponse<Blob>> {
        return this.http.get(`${this.apiUrl}/picture/${id}`, { responseType: 'blob', observe: 'response' });
    }
}