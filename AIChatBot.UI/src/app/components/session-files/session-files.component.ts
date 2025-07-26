import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ChatSession } from '../../entities/chatsession';
import { AgentFile } from '../../entities/agent-file';
import { FileService } from '../../services/file.service';

@Component({
  selector: 'app-session-files',
  templateUrl: './session-files.component.html',
  styleUrls: ['./session-files.component.css'],
  standalone: false
})
export class SessionFilesComponent implements OnChanges {
  @Input() selectedSession?: ChatSession;
  @Input() userId?: string;
  
  files: AgentFile[] = [];
  isLoading = false;
  errorMessage = '';

  constructor(private fileService: FileService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['selectedSession'] || changes['userId']) {
      this.loadSessionFiles();
    }
  }

  private loadSessionFiles(): void {
    if (!this.selectedSession || !this.userId) {
      this.files = [];
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.fileService.getSessionFiles(this.selectedSession.id, this.userId).subscribe({
      next: (files) => {
        this.files = files;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading session files:', error);
        this.errorMessage = 'Failed to load files';
        this.isLoading = false;
        this.files = [];
      }
    });
  }

  downloadFile(file: AgentFile): void {
    if (!this.userId) return;

    const downloadUrl = this.fileService.downloadFileUrl(file.id, this.userId);
    window.open(downloadUrl, '_blank');
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 B';
    const k = 1024;
    const sizes = ['B', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }

  formatDate(dateString: string): string {
    const date = new Date(dateString);
    return date.toLocaleDateString() + ' ' + date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }
}