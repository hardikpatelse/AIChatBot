import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule } from '@angular/forms'
import { RouterModule, Routes } from '@angular/router'
import { Chat } from './chat.component'
import { TypingIndicatorModule } from '../typing-indicator/typing-indicator.module'
import { ModelSelectorModule } from '../model-selector/model-selector.module'

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
        TypingIndicatorModule,
        ModelSelectorModule,
        RouterModule.forChild(routes)
    ]
})
export class ChatModule { }