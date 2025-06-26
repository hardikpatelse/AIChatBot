import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { ChatModeSelectorComponent } from './chat-mode-selector.component'

@NgModule({
    declarations: [ChatModeSelectorComponent],
    imports: [CommonModule],
    exports: [ChatModeSelectorComponent]
})
export class ChatModeSelectorModule { }
