import { ChatSession } from './chatsession'

export interface User {
    id: string
    email: string
    name: string
    chatSessions: ChatSession[]
}
