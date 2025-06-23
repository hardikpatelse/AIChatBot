import { Component, OnInit, ChangeDetectorRef, ViewChild, ElementRef, AfterViewInit } from '@angular/core'
import { ChatService } from '../../services/chat.service'
import { Model } from '../../entities/model'
import { ChatHistoryResponse, ChatMessage } from '../../entities/chat-history'
import { marked } from 'marked'

@Component({
  selector: 'app-chat',
  standalone: false,
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class Chat implements OnInit, AfterViewInit {
  models: Model[] = [];
  selectedModelId: string = '';
  selectedModelDetails: Model | null = null;
  chatHistory: ChatMessage[] = [];
  userMessage: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';

  @ViewChild('chatHistoryContainer') chatHistoryContainer!: ElementRef

  constructor(private chatService: ChatService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.chatService.getModels().subscribe({
      next: (models: Model[]) => {
        this.models = models
      },
      error: () => {
        this.errorMessage = 'Failed to load models.'
      }
    })
  }

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

  onModelChange(modelId: string): void {
    this.selectedModelId = modelId
    this.selectedModelDetails = this.models.find(m => m.id === modelId) || null
    this.loadHistory()
  }

  loadHistory(): void {
    this.chatService.getHistory(this.selectedModelId).subscribe({
      next: (modelHistory: ChatHistoryResponse) => {
        this.chatHistory = modelHistory.history || []
        this.errorMessage = ''
        this.cdr.detectChanges()
        setTimeout(() => this.scrollToBottom(), 0) // Ensure DOM is updated before scrolling
      },
      error: () => {
        this.errorMessage = 'Failed to load chat history.'
      }
    })
  }

  sendMessage(): void {
    if (!this.userMessage.trim() || this.isLoading) return
    const msg = this.userMessage
    const now = new Date().toISOString()
    this.chatHistory.push({ role: 'user', content: msg, dateTime: now })
    this.userMessage = ''
    this.isLoading = true
    this.errorMessage = ''
    this.cdr.detectChanges()
    setTimeout(() => this.scrollToBottom(), 0)
    this.chatService.sendMessage(this.selectedModelId, msg).subscribe({
      next: res => {
        this.chatHistory.push({ role: 'assistant', content: res.response, dateTime: new Date().toISOString() })
        this.isLoading = false
        this.cdr.detectChanges()
        setTimeout(() => this.scrollToBottom(), 0)
      },
      error: () => {
        this.errorMessage = 'Failed to send message to server.'
        this.isLoading = false
      }
    })
  }

  parseMarkdown(content: string): any {
    return marked.parse(content)
  }
}
