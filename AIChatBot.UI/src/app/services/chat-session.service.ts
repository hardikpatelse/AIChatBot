import { Injectable } from '@angular/core'
import { HttpClient } from '@angular/common/http'
import { Observable } from 'rxjs'

export interface ChatSession {
    id: string
    name: string
    // Add other properties as needed
}

@Injectable({
    providedIn: 'root'
})
export class ChatSessionService {
    private apiUrl = 'https://localhost:7154/api';

    constructor(private http: HttpClient) { }

    createSession(userId: string, name: string, modelId: number): Observable<ChatSession> {
        return this.http.post<ChatSession>(`${this.apiUrl}/ChatSession/create`, { 'name': name, 'userId': userId, 'modelId': modelId })
    }

    getSessions(userId: string): Observable<ChatSession[]> {
        return this.http.get<ChatSession[]>(`${this.apiUrl}/ChatSession/list?userId=${encodeURIComponent(userId)}`)
    }
}
