import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
import { Observable } from 'rxjs/internal/Observable'
import { Model } from '../entities/model'
import { ChatHistoryResponse } from '../entities/chat-history'

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiUrl = 'https://localhost:7154';

  constructor(private http: HttpClient) { }

  getModels(): Observable<Model[]> {
    return this.http.get<Model[]>(`${this.apiUrl}/models`)
  }

  getHistory(userId: string, chatSessionIdentity: string, modelId: number): Observable<ChatHistoryResponse> {
    return this.http.get<ChatHistoryResponse>(`${this.apiUrl}/chat/history?userId=${encodeURIComponent(userId)}&chatSessionIdentity=${encodeURIComponent(chatSessionIdentity)}&modelId=${modelId}`)
  }

  sendMessage(userId: string, chatSessionIdentity: string, modelId: number, message: string, aIMode: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/chat`, { userId, chatSessionIdentity, modelId, message, aIMode })
  }
}
