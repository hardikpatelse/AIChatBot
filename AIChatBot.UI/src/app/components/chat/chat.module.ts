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
        ChatHistoryModule
    ]
})
export class ChatModule { }