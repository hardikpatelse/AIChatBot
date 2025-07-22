import { Component, Input, Output, EventEmitter } from '@angular/core'

export interface ChatSession {
    id: string
    name: string
    // Add other properties as needed
}

@Component({
    selector: 'app-chat-session-list',
    templateUrl: './chat-session-list.component.html',
    styleUrls: ['./chat-session-list.component.css'],
    standalone: false
})
export class ChatSessionListComponent {
    @Input() sessions: ChatSession[] = [];
    @Output() sessionSelected = new EventEmitter<ChatSession>();
    @Output() newChat = new EventEmitter<void>();

    onSessionClick(session: ChatSession) {
        this.sessionSelected.emit(session)
    }

    onNewChatClick() {
        this.newChat.emit()
    }
}
