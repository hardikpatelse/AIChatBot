import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule } from '@angular/forms'
import { NewChatSessionComponent } from './new-chat-session.component'
import { ModelSelectorModule } from '../model-selector/model-selector.module'

@NgModule({
    declarations: [NewChatSessionComponent],
    imports: [CommonModule, FormsModule, ModelSelectorModule],
    exports: [NewChatSessionComponent]
})
export class NewChatSessionModule { }
