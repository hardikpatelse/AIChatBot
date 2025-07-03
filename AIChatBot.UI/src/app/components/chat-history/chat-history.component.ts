import { Component, Input, Output, EventEmitter, ViewChild, ElementRef, AfterViewInit } from '@angular/core'
import { ChatMessage } from '../../entities/chat-history'

@Component({
    selector: 'app-chat-history',
    templateUrl: './chat-history.component.html',
    styleUrls: ['./chat-history.component.css'],
    standalone: false
})
export class ChatHistoryComponent implements AfterViewInit {
    @Input() chatHistory: ChatMessage[] = [];
    @Input() isLoading: boolean = false;
    @Input() errorMessage: string = '';
    @Input() parseMarkdown!: (content: string) => string

    @ViewChild('chatHistoryContainer') chatHistoryContainer!: ElementRef

    ngAfterViewInit(): void {
        // Use MutationObserver to watch for changes in chatHistoryContainer
        if (this.chatHistoryContainer) {
            const observer = new MutationObserver(() => {
                this.scrollToBottom()
            })
            observer.observe(this.chatHistoryContainer.nativeElement, { childList: true, subtree: true })
        }
        this.scrollToBottom()
    }

    private scrollToBottom(): void {
        if (this.chatHistoryContainer) {
            setTimeout(() => {
                this.chatHistoryContainer.nativeElement.scrollTop = this.chatHistoryContainer.nativeElement.scrollHeight
            })
        }
    }
}
