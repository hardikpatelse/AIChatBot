import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { TypingIndicator } from './typing-indicator'

@NgModule({
    declarations: [TypingIndicator],
    imports: [CommonModule],
    exports: [TypingIndicator]
})
export class TypingIndicatorModule { }
