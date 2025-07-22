import { Component, OnInit, ChangeDetectorRef, ViewChild, ElementRef, AfterViewInit } from '@angular/core'
import { ChatService } from '../../services/chat.service'
import { Model } from '../../entities/model'
import { ChatHistoryResponse, ChatMessage } from '../../entities/chat-history'
import { marked } from 'marked'
import { User } from '../../services/user.service'
import { ChatMode } from '../../entities/chatmode'
import { AIModelChatMode } from '../../entities/aimodel-chatmode'

@Component({
  selector: 'app-chat',
  standalone: false,
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class Chat implements OnInit {
  models: Model[] = [];
  selectedModelId: number = 0;
  selectedModelDetails: Model | null = null;
  chatHistory: ChatMessage[] = [];
  userMessage: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';
  selectedChatMode: string = 'chat';
  chatModes: AIModelChatMode[] = [];
  userId?: string
  chatSessionIdentity?: string
  userObj!: User
  userName: string = '';
  showNewChatModal: boolean = false;

  constructor(private chatService: ChatService) { }
  //, private cdr: ChangeDetectorRef

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

  onModelChange(model: Model): void {
    if (!model || !model.id) {
      this.selectedModelId = 0
      this.selectedModelDetails = null
      this.chatModes = []
    } else {
      this.selectedModelId = model.id
      this.selectedModelDetails = this.models.find(m => m.id === model.id) || null
      this.chatModes = (this.selectedModelDetails && Array.isArray((this.selectedModelDetails as any).chatModes))
        ? (this.selectedModelDetails as any).chatModes
        : []
      console.log('Selected Model:', this.selectedModelDetails)
      console.log('Chat Modes:', this.chatModes)
      // this.loadHistory();
    }
  }

  loadHistory(): void {
    this.chatService.getHistory(this.selectedModelId).subscribe({
      next: (modelHistory: ChatHistoryResponse) => {
        this.chatHistory = modelHistory.history || []
        this.errorMessage = ''
        // this.cdr.detectChanges()
      },
      error: () => {
        this.errorMessage = 'Failed to load chat history.'
      }
    })
  }

  sendMessage(): void {
    if (!this.userMessage.trim() || this.isLoading || !this.userId || !this.chatSessionIdentity) return
    const msg = this.userMessage
    const now = new Date().toISOString()
    this.chatHistory.push({ role: 'user', content: msg, dateTime: now })
    this.userMessage = ''
    this.isLoading = true
    this.errorMessage = ''
    // this.cdr.detectChanges()
    this.chatService.sendMessage(this.userId, this.chatSessionIdentity, this.selectedModelId, msg, this.selectedChatMode).subscribe({
      next: res => {
        this.chatHistory.push({ role: 'assistant', content: res.response, dateTime: new Date().toISOString() })
        this.isLoading = false
        // this.cdr.detectChanges()
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

  onUserFormSubmit(user: any) {
    this.userObj = user
    this.userName = user.name || ''
    this.userId = user.id
    if (user.chatSessions?.length > 0) {
      // Select the most recent chat session or handle as needed
      const mostRecentSession = this.getMostRecentSession(user.chatSessions);
      this.chatSessionIdentity = mostRecentSession?.id || user.chatSessions?.[0]?.id;
    } else {
      // Automatically show new chat modal after registration/start
      this.showNewChatModal = true
    }
    // Optionally set chatSessionIdentity or other properties here
  }

  onSessionSelected(session: any) {
    // Load chat history by session.id or perform any action needed
    // Example: this.loadHistoryBySession(session.id);
  }

  onNewChat(session: any) {
    this.showNewChatModal = true
    console.log('New chat session requested:', session)
    // Optionally handle session info if needed
  }

  onNewChatSessionCreated(session: any) {
    this.showNewChatModal = false
    // Optionally add the new session to the session list or reload sessions
  }
}
