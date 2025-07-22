import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ChatSessionListComponent } from './chat-session-list.component'

@NgModule({
    declarations: [ChatSessionListComponent],
    imports: [CommonModule],
    exports: [ChatSessionListComponent]
})
export class ChatSessionListModule { }
