import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({ providedIn: 'root' })
export class SignalRService {
  private hubConnection!: signalR.HubConnection;
  public statusUpdateCallback: (status: string) => void = () => {};

  startConnection(userId: string) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7154/chatHub?user=' + userId)
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().then(() => {
      console.log('SignalR Connected');
    }).catch(err => {
      console.error('SignalR Connection Error: ', err);
    });

    this.hubConnection.on('ReceiveStatus', (status: string) => {
      this.statusUpdateCallback(status);
    });
  }

  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop();
    }
  }

  getConnectionId(): string | null {
    return this.hubConnection?.connectionId || null;
  }
}