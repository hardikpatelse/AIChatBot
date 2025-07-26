import { HttpClient } from '@angular/common/http'
import { Injectable } from '@angular/core'
// Removed duplicate import
import { Observable } from 'rxjs'
import { Model } from '../entities/model'
import { ChatHistoryResponse } from '../entities/chat-history'
import { environment } from '../../environments/environment'

@Injectable({
  providedIn: 'root'
})
export class ChatService {
  private apiUrl = environment.apiBaseUrl + '/api';

  constructor(private http: HttpClient) { }

  getModels(): Observable<Model[]> {
    return this.http.get<Model[]>(`${this.apiUrl}/models`)
  }

  getHistory(userId: string, chatSessionIdentity: string, modelId: number): Observable<ChatHistoryResponse> {
    return this.http.get<ChatHistoryResponse>(`${this.apiUrl}/chat/history?userId=${encodeURIComponent(userId)}&chatSessionIdentity=${encodeURIComponent(chatSessionIdentity)}&modelId=${modelId}`)
  }

  sendMessage(userId: string, chatSessionIdentity: string, modelId: number, message: string, aIMode: string, connectionId?: string): Observable<any> {
    const payload: any = { userId, chatSessionIdentity, modelId, message, aIMode }
    if (connectionId) {
      payload.connectionId = connectionId
    }
    return this.http.post<any>(`${this.apiUrl}/chat`, payload)
  }
}
