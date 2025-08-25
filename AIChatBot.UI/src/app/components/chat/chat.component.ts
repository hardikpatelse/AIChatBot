import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core'
import { ChatService } from '../../services/chat.service'
import { SignalRService } from '../../services/signalr.service'
import { Model } from '../../entities/model'
import { ChatHistoryResponse, ChatMessage } from '../../entities/chat-history'
import { marked } from 'marked'
import { User } from '../../entities/user'
import { AIModelChatMode } from '../../entities/aimodel-chatmode'
import { ChatSession } from '../../entities/chatsession'
import { Subscription } from 'rxjs/internal/Subscription'

@Component({
  selector: 'app-chat',
  standalone: false,
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.css'
})
export class Chat implements OnInit, OnDestroy {
  models: Model[] = [];
  selectedModelId: number = 0;
  selectedModelDetails: Model | null = null;
  chatHistory!: ChatMessage[]
  userMessage: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';
  selectedChatMode: string = 'chat';
  chatModes: AIModelChatMode[] = [];
  userId?: string
  chatSessionIdentity?: string
  selectedSession?: any
  userObj!: User
  userName: string = '';
  showNewChatModal: boolean = false;
  statusMessages: string[] = [];
  connectionId?: string | null

  private messageSub?: Subscription

  constructor(private chatService: ChatService, private signalRService: SignalRService, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.chatService.getModels().subscribe({
      next: (models: Model[]) => {
        this.models = models
      },
      error: () => {
        this.errorMessage = 'Failed to load models.'
      }
    })
    this.messageSub = this.signalRService.message$.subscribe(message => {
      if (message) {
        this.chatHistory.push(message)
        this.cdr.detectChanges() // Ensure view updates with new message
      }
    })
  }

  ngOnDestroy(): void {
    this.signalRService.stopConnection()
    this.messageSub?.unsubscribe()
  }

  // Initialize SignalR status update callback
  initializeSignalR(): void {
    this.signalRService.statusUpdateCallback = (status: string) => {
      this.statusMessages.push(status)
      this.cdr.detectChanges()
    }
    this.signalRService.messageUpdateCallback = (message: ChatMessage) => {
      this.chatHistory.push(message)
      this.cdr.detectChanges()
    }
  }

  onModelChange(model: Model): void {
    if (!model) {
      this.selectedModelId = 0
      this.selectedModelDetails = null
      this.chatModes = []
    } else {
      this.selectedModelId = model.id
      this.selectedModelDetails = this.models.find(m => m.id === model.id) || null
      this.chatModes = (this.selectedModelDetails && Array.isArray((this.selectedModelDetails as any).chatModes))
        ? (this.selectedModelDetails as any).chatModes
        : []
      const savedMode = localStorage.getItem('selectedChatMode')
      if (savedMode && this.chatModes.some(opt => opt.chatMode.mode === savedMode)) {
        this.selectedChatMode = savedMode
      }
    }
  }

  loadHistory(): void {
    if (!this.userId || !this.chatSessionIdentity) return
    // Get SignalR connection ID
    this.connectionId = this.signalRService.getConnectionId()
    this.chatService.getHistory(this.userId, this.chatSessionIdentity, this.selectedModelId).subscribe({
      next: (session: ChatHistoryResponse) => {
        this.selectedSession = session
        this.chatHistory = session.messages || []
        this.errorMessage = ''
        this.cdr.detectChanges() // Ensure view updates after loading history
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
    this.chatHistory.push({
      role: 'user', content: msg, timeStamp: now,
      id: 0,
      chatSessionId: this.selectedSession?.id || 0
    })
    this.userMessage = ''
    this.isLoading = true
    this.errorMessage = ''
    // Clear previous status messages
    this.statusMessages = []

    this.chatService.sendMessage(this.userId, this.chatSessionIdentity, this.selectedModelId, msg, this.selectedChatMode, this.connectionId || undefined).subscribe({
      next: res => {
        if (res.showInHistory) {
          this.chatHistory.push({
            role: 'assistant', content: res.response, timeStamp: new Date().toISOString(),
            id: 0,
            chatSessionId: this.selectedSession?.id || 0
          })
        }
        this.isLoading = false
        // Clear status messages after completion
        this.statusMessages = []
        this.cdr.detectChanges() // Ensure view updates after loading history
      },
      error: () => {
        this.errorMessage = 'Failed to send message to server.'
        this.isLoading = false
        // Clear status messages on error
        this.statusMessages = []
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
      this.chatSessionIdentity = user.chatSessions?.[0]?.uniqueIdentity
    } else {
      // Automatically show new chat modal after registration/start
      this.showNewChatModal = true
    }
    // Optionally set chatSessionIdentity or other properties here

    // Start SignalR connection and initialize callbacks
    if (this.userId) {
      this.signalRService.startConnection(this.userId)
    }
  }

  onSessionSelected(session: any) {
    // Set the selected session and load its chat history
    this.selectedSession = session
    this.chatSessionIdentity = session.uniqueIdentity
    this.loadHistory()
  }

  onNewChat(session: any) {
    this.showNewChatModal = true
    // Optionally handle session info if needed
  }

  onNewChatSessionCreated(session: any) {
    this.showNewChatModal = false
    // Set the newly created session as selected and load its history (which will be empty initially)
    if (session) {
      this.refreshUserSessions(session)
      this.onSessionSelected(session)
    }
  }

  private refreshUserSessions(session: ChatSession) {
    if (this.userObj && !this.userObj.chatSessions.some(s => s.id === session.id)) {
      this.userObj.chatSessions.push(session)
    }
  }
}
