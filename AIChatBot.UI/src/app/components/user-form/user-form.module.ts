import { NgModule } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule } from '@angular/forms'
import { UserFormComponent } from './user-form.component'
import { ReactiveFormsModule } from '@angular/forms'

@NgModule({
    declarations: [UserFormComponent],
    imports: [CommonModule, FormsModule, ReactiveFormsModule],
    exports: [UserFormComponent]
})
export class UserFormModule { }
