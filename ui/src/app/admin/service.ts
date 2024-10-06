import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { values } from 'constant';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = values.backend_address + '/api/Auth/verify-password';

    constructor(private http: HttpClient) { }

    authenticate(password: string): Observable<any> {
        return this.http.post<any>(this.apiUrl, { password });
    }
}