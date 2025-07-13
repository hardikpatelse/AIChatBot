import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule } from '@angular/forms'
import { ChatModeSelectorComponent } from './chat-mode-selector.component'

@NgModule({
    declarations: [ChatModeSelectorComponent],
    imports: [CommonModule, FormsModule],
    exports: [ChatModeSelectorComponent]
})
export class ChatModeSelectorModule { }
