import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AgentFile } from '../entities/agent-file';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileService {
  private apiUrl = environment.apiBaseUrl;

  constructor(private http: HttpClient) { }

  getSessionFiles(sessionId: number, userId: string): Observable<AgentFile[]> {
    const params = new HttpParams().set('userId', userId);
    return this.http.get<AgentFile[]>(`${this.apiUrl}/api/files/${sessionId}`, { params });
  }

  downloadFile(fileId: number, userId: string): Observable<Blob> {
    const params = new HttpParams().set('userId', userId);
    return this.http.get(`${this.apiUrl}/api/files/download/${fileId}`, { 
      params,
      responseType: 'blob'
    });
  }

  downloadFileUrl(fileId: number, userId: string): string {
    return `${this.apiUrl}/api/files/download/${fileId}?userId=${encodeURIComponent(userId)}`;
  }
}