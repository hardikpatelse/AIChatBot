import { Component, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-document-upload',
  standalone: false,
  templateUrl: './document-upload.component.html',
  styleUrls: ['./document-upload.component.css']
})
export class DocumentUploadComponent {
  @Input() userId: string = '';
  
  uploadStatus: string = '';
  isUploading: boolean = false;
  uploadedFiles: any[] = [];
  
  constructor(private http: HttpClient) {}

  ngOnInit() {
    if (this.userId) {
      this.loadUserDocuments();
    }
  }

  onFileSelect(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.uploadFile(file);
    }
  }

  onFileDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    
    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.uploadFile(files[0]);
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
  }

  private uploadFile(file: File) {
    if (!this.isValidFileType(file.name)) {
      this.uploadStatus = 'Unsupported file type. Please use .txt, .md, or .pdf files.';
      return;
    }

    this.isUploading = true;
    this.uploadStatus = 'Uploading...';

    const formData = new FormData();
    formData.append('file', file);
    formData.append('userId', this.userId);

    this.http.post(`${environment.apiBaseUrl}/api/documents/upload`, formData).subscribe({
      next: (response: any) => {
        this.uploadStatus = `✓ ${response.fileName} uploaded successfully!`;
        this.isUploading = false;
        this.loadUserDocuments();
      },
      error: (error) => {
        this.uploadStatus = `Error: ${error.error?.message || error.message}`;
        this.isUploading = false;
      }
    });
  }

  private loadUserDocuments() {
    this.http.get(`${environment.apiBaseUrl}/api/documents/${this.userId}/documents`).subscribe({
      next: (response: any) => {
        this.uploadedFiles = response.documents || [];
      },
      error: (error) => {
        console.error('Error loading documents:', error);
      }
    });
  }

  deleteDocument(docId: string) {
    if (confirm('Are you sure you want to delete this document?')) {
      this.http.delete(`${environment.apiBaseUrl}/api/documents/${this.userId}/documents/${docId}`).subscribe({
        next: () => {
          this.uploadStatus = 'Document deleted successfully';
          this.loadUserDocuments();
        },
        error: (error) => {
          this.uploadStatus = `Error deleting document: ${error.message}`;
        }
      });
    }
  }

  private isValidFileType(fileName: string): boolean {
    const allowedExtensions = ['.txt', '.md', '.pdf'];
    const extension = fileName.toLowerCase().substring(fileName.lastIndexOf('.'));
    return allowedExtensions.includes(extension);
  }
}