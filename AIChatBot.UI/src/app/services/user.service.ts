import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'
import { User } from '../entities/user'

@Injectable({
    providedIn: 'root'
})
export class UserService {
    private apiUrl = 'https://localhost:7154/api';

    constructor(private http: HttpClient) { }

    getUserByEmail(email: string): Observable<User> {
        return this.http.get<User>(`${this.apiUrl}/user/by-email?email=${encodeURIComponent(email)}`)
    }

    registerUser(name: string, email: string): Observable<User> {
        return this.http.post<User>(`${this.apiUrl}/user/register`, { 'name': name, 'email': email })
    }
}

