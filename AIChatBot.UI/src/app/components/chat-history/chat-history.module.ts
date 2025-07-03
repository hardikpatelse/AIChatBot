import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ChatHistoryComponent } from './chat-history.component'
import { TypingIndicatorModule } from '../typing-indicator/typing-indicator.module'

@NgModule({
    declarations: [ChatHistoryComponent],
    imports: [CommonModule, TypingIndicatorModule],
    exports: [ChatHistoryComponent]
})
export class ChatHistoryModule { }
