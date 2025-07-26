import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { User } from '../entities/user'
import { environment } from '../../environments/environment'
// Removed duplicate import
import { Observable } from 'rxjs'

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private apiUrl = environment.apiBaseUrl + '/api';

    constructor(private http: HttpClient) { }

    getUserByEmail(email: string): Observable<User> {
        return this.http.get<User>(`${this.apiUrl}/user/by-email?email=${encodeURIComponent(email)}`)
    }

    registerUser(name: string, email: string): Observable<User> {
        return this.http.post<User>(`${this.apiUrl}/user/register`, { 'name': name, 'email': email })
    }
}

