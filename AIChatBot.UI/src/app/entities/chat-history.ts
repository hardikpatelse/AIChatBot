export interface ChatMessage {
    role: 'user' | 'assistant'
    content: string
    dateTime: string
}

export interface ChatHistoryResponse {
    modelId: string
    history: ChatMessage[]
}
