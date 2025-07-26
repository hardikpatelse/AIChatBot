import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { ChatSession } from '../entities/chatsession'
import { environment } from '../../environments/environment'
// Removed duplicate import
import { Observable } from 'rxjs'

@Injectable({
    providedIn: 'root'
})
export class ChatSessionService {
    private apiUrl = environment.apiBaseUrl + '/api';

    constructor(private http: HttpClient) { }

    createSession(userId: string, name: string): Observable<ChatSession> {
        return this.http.post<ChatSession>(`${this.apiUrl}/ChatSession/create`, { 'name': name, 'userId': userId })
    }

    getSessions(userId: string): Observable<ChatSession[]> {
        return this.http.get<ChatSession[]>(`${this.apiUrl}/ChatSession/list?userId=${encodeURIComponent(userId)}`)
    }
}
