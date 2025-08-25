import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { DocumentUploadComponent } from './document-upload.component';

@NgModule({
  declarations: [
    DocumentUploadComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    HttpClientModule
  ],
  exports: [
    DocumentUploadComponent
  ]
})
export class DocumentUploadModule { }