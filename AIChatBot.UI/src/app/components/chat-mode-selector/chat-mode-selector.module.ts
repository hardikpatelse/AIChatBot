import { EventEmitter, Input, NgModule, Output } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule } from '@angular/forms'
import { ChatModeSelectorComponent } from './chat-mode-selector.component'
import { ChatMode } from '../../entities/chatmode'

@NgModule({
    declarations: [ChatModeSelectorComponent],
    imports: [CommonModule, FormsModule],
    exports: [ChatModeSelectorComponent]
})
export class ChatModeSelectorModule { }
