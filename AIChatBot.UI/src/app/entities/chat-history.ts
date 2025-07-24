export interface ChatMessage {
    id: number
    chatSessionId: number
    role: string
    content: string
    timeStamp: string
}

export interface ChatHistoryResponse {
    id: number
    name: string
    uniqueIdentity: string
    userId: string
    createdAt: string
    messages: ChatMessage[]
}
