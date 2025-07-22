import { ChatMode } from "./chatmode"
import { Model } from "./model"

export interface AIModelChatMode {
    aiModelId: number
    aiModel: Model | null
    chatModeId: number
    chatMode: ChatMode
}