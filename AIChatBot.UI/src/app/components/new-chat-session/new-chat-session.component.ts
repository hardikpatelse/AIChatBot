import { Component, Input, Output, EventEmitter } from '@angular/core'
import { ChatSessionService, ChatSession } from '../../services/chat-session.service'
import { Model } from '../../entities/model'

@Component({
    selector: 'app-new-chat-session',
    templateUrl: './new-chat-session.component.html',
    styleUrls: ['./new-chat-session.component.css'],
    standalone: false
})
export class NewChatSessionComponent {
    @Input() userId?: string
    @Output() sessionCreated = new EventEmitter<ChatSession>();
    chatName: string = '';
    isLoading: boolean = false;
    errorMessage: string = '';
    selectedModel: Model | null = null;

    constructor(private chatSessionService: ChatSessionService) { }

    createSession() {
        if (!this.chatName.trim() || !this.userId || !this.selectedModel) return
        this.isLoading = true
        this.errorMessage = ''
        this.chatSessionService.createSession(this.userId, this.chatName, this.selectedModel.id).subscribe({
            next: (session) => {
                this.sessionCreated.emit(session)
                this.chatName = ''
                this.isLoading = false
            },
            error: (err) => {
                this.errorMessage = 'Failed to create chat session.'
                this.isLoading = false
            }
        })
    }

    onModelSelected(model: Model) {
        this.selectedModel = model
        // You can use selectedModel when creating the session if needed
    }
}
