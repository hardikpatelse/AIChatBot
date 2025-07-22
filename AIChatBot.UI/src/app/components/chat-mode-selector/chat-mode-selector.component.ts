import { Component, EventEmitter, Input, Output } from '@angular/core'
import { ChatMode } from '../../entities/chatmode'
import { AIModelChatMode } from '../../entities/aimodel-chatmode'

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
    @Input() supportedModes: AIModelChatMode[] = [];
    @Output() modeSelected = new EventEmitter<string>();

    selectedMode: string = 'chat';

    constructor() {

    }

    ngOnInit() {
        const savedMode = localStorage.getItem('selectedChatMode')
        if (savedMode && this.supportedModes.some(opt => opt.chatMode.mode === savedMode)) {
            this.selectMode(savedMode)
        }
    }

    selectMode(mode: string) {
        this.selectedMode = mode
        localStorage.setItem('selectedChatMode', mode)
        this.modeSelected.emit(mode)
    }
}
