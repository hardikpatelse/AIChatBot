import { Injectable } from '@angular/core'
import * as signalR from '@microsoft/signalr'
import { BehaviorSubject } from 'rxjs'
import { ChatMessage } from '../entities/chat-history'
import { environment } from '../../environments/environment'

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private hubConnection!: signalR.HubConnection
  public statusUpdateCallback: (status: string) => void = () => { };
  public messageUpdateCallback: (message: ChatMessage) => void = () => { };

  public status$: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public message$: BehaviorSubject<ChatMessage | null> = new BehaviorSubject<ChatMessage | null>(null);

  startConnection(userId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(environment.apiBaseUrl + '/chatHub?user=' + userId)
      .withAutomaticReconnect()
      .build()

    this.hubConnection.start().then(() => {
      console.log('SignalR Connected')
    }).catch(err => {
      console.error('SignalR Connection Error: ', err)
    })

    this.hubConnection.on('ReceiveStatus', (status: string) => {
      this.statusUpdateCallback(status)
      this.status$.next(status)
    })

    this.hubConnection.on('ReceiveMessage', (message: ChatMessage) => {
      this.messageUpdateCallback(message)
      this.message$.next(message)
    })
  }

  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop()
    }
  }

  getConnectionId(): string | null {
    return this.hubConnection?.connectionId || null
  }
}