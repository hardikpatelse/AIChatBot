import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule } from '@angular/forms'
import { ModelSelectorComponent } from './model-selector.component'

@NgModule({
    declarations: [ModelSelectorComponent],
    imports: [CommonModule, FormsModule],
    exports: [ModelSelectorComponent]
})
export class ModelSelectorModule { }