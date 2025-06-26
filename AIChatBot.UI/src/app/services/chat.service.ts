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

  getHistory(modelId: string): Observable<ChatHistoryResponse> {
    return this.http.get<ChatHistoryResponse>(`${this.apiUrl}/chat/history?modelId=${modelId}`)
  }

  sendMessage(model: string, message: string, aIMode: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/chat`, { model, message, aIMode })
  }
}
