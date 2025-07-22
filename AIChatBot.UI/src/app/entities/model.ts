import { AIModelChatMode } from "./aimodel-chatmode"

export interface Model {
    id: number
    name: string
    company?: string
    logoUrl?: string
    description?: string
    referralSource?: string
    referenceLink?: string
    aiModelChatModes: AIModelChatMode[]
}
