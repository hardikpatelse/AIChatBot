import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule } from '@angular/forms'
import { RouterModule, Routes } from '@angular/router'
import { Chat } from './chat.component'
import { TypingIndicatorModule } from '../typing-indicator/typing-indicator.module'
import { ModelSelectorModule } from '../model-selector/model-selector.module'
import { ChatModeSelectorModule } from '../chat-mode-selector/chat-mode-selector.module'
import { ModelDetailsModule } from '../model-details/model-details.module'
import { ChatHistoryModule } from '../chat-history/chat-history.module'
import { UserFormModule } from '../user-form/user-form.module'
import { HeaderModule } from '../header/header.module'
import { ChatSessionListModule } from '../chat-session-list/chat-session-list.module'
import { NewChatSessionModule } from '../new-chat-session/new-chat-session.module'
import { DocumentUploadModule } from '../document-upload/document-upload.module'

const routes: Routes = [
    { path: '', component: Chat }
]

@NgModule({
    declarations: [
        Chat
    ],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule.forChild(routes),
        TypingIndicatorModule,
        ModelSelectorModule,
        ChatModeSelectorModule,
        ModelDetailsModule,
        ChatHistoryModule,
        UserFormModule,
        HeaderModule,
        ChatSessionListModule,
        NewChatSessionModule,
        DocumentUploadModule
    ]
})
export class ChatModule { }