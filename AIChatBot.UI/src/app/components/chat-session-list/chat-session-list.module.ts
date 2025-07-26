import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ChatSessionListComponent } from './chat-session-list.component'
import { SessionFilesModule } from '../session-files/session-files.module'

@NgModule({
    declarations: [ChatSessionListComponent],
    imports: [
        CommonModule,
        SessionFilesModule
    ],
    exports: [ChatSessionListComponent]
})
export class ChatSessionListModule { }
