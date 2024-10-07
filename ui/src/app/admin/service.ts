import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { values } from 'constant';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = values.backend_address + '/api/studentlicense';
    private authUrl = values.backend_address + '/api/auth';

    constructor(private http: HttpClient) { }

    login(password: string): Observable<any> {
        return this.http.post<any>(`${this.authUrl}/login`, { password });
    }

    validateToken(token: string): Observable<any> {
        const headers = new HttpHeaders({
            'Authorization': `Bearer ${token}`
        });
        return this.http.get<any>(`${this.authUrl}/validate-token`, { headers });
    }

    getApplications(): Observable<any[]> {
        const headers = this.getAuthHeaders();
        return this.http.get<any[]>(this.apiUrl, { headers });
    }

    editApplication(id: number, application: any): Observable<any> {
        const headers = this.getAuthHeaders();
        return this.http.put<any>(`${this.apiUrl}/${id}`, application, { headers });
    }

    deleteApplication(id: number): Observable<any> {
        const headers = this.getAuthHeaders();
        return this.http.delete<any>(`${this.apiUrl}/${id}`, { headers });
    }

    getPicture(id: number): Observable<HttpResponse<Blob>> {
        const headers = this.getAuthHeaders();
        return this.http.get(`${this.apiUrl}/picture/${id}`, { responseType: 'blob', observe: 'response', headers });
    }

    private getAuthHeaders(): HttpHeaders {
        const token = localStorage.getItem('token');
        return new HttpHeaders({
            'Authorization': `Bearer ${token}`
        });
    }
}