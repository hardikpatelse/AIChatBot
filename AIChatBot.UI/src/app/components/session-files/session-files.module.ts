import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { SessionFilesComponent } from './session-files.component';

@NgModule({
  declarations: [
    SessionFilesComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  exports: [
    SessionFilesComponent
  ]
})
export class SessionFilesModule { }