import { Component, EventEmitter, Input, Output } from '@angular/core'

interface ChatModeOption {
    value: string
    label: string
    icon: string
    description: string
}

@Component({
    selector: 'app-chat-mode-selector',
    standalone: false,
    templateUrl: './chat-mode-selector.component.html',
    styleUrls: ['./chat-mode-selector.component.css']
})
export class ChatModeSelectorComponent {
    @Input() supportedModes: string[] = ['chat', 'tools'];
    @Output() modeSelected = new EventEmitter<string>();

    options: ChatModeOption[] = [
        { value: 'chat', label: 'Chat-Only', icon: '🧠', description: 'Minimalist Q&A' },
        { value: 'tools', label: 'AI + Tools', icon: '🛠️', description: 'Task executor' },
        { value: 'agent', label: 'AI Agent', icon: '🤖', description: 'Full planner/agent' }
    ];

    selectedMode: string = 'chat';

    ngOnInit() {
        const savedMode = localStorage.getItem('selectedChatMode')
        if (savedMode && this.options.some(opt => opt.value === savedMode)) {
            this.selectMode(savedMode)
        }
    }

    selectMode(mode: string) {
        this.selectedMode = mode
        localStorage.setItem('selectedChatMode', mode)
        this.modeSelected.emit(mode)
    }
}
