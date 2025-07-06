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
export class Chat implements OnInit {
  models: Model[] = [];
  selectedModelId: string = '';
  selectedModelDetails: Model | null = null;
  chatHistory: ChatMessage[] = [];
  userMessage: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';
  selectedChatMode: string = 'chat';

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
    this.chatService.sendMessage(this.selectedModelId, msg, this.selectedChatMode).subscribe({
      next: res => {
        this.chatHistory.push({ role: 'assistant', content: res.response, dateTime: new Date().toISOString() })
        this.isLoading = false
        this.cdr.detectChanges()
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

  onModeSelected(mode: string): void {
    this.selectedChatMode = mode
    // You can add additional logic here if needed
  }
}
