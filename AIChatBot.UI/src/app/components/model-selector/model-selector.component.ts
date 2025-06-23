import { Component, EventEmitter, OnInit, Output } from '@angular/core'
import { Model } from '../../entities/model'
import { ChatService } from '../../services/chat.service'

@Component({
  selector: 'app-model-selector',
  standalone: false,
  templateUrl: './model-selector.component.html',
  styleUrl: './model-selector.component.css'
})
export class ModelSelectorComponent implements OnInit {
  models: Model[] = [];
  selectedModelId: string = '';
  showDetails = false;

  @Output() modelSelected = new EventEmitter<string>();

  constructor(private chatService: ChatService) {

  }

  ngOnInit(): void {
    this.getModels()

    const saved = localStorage.getItem('selectedModel')
    this.selectedModelId = saved || this.models[0]?.id
    this.modelSelected.emit(this.selectedModelId)
  }

  getModels() {
    this.chatService.getModels().subscribe(data => {
      this.models = data
      const savedModel = localStorage.getItem('selectedModel')
      if (this.models.length > 0) {
        if (savedModel && this.models.some(m => m.id === savedModel)) {
          this.selectedModelId = savedModel
        } else {
          this.selectedModelId = this.models[0].id

        }
        this.modelSelected.emit(this.selectedModelId)
      } else {
        this.selectedModelId = ''
      }
    })
  }

  onModelChange() {
    localStorage.setItem('selectedModel', this.selectedModelId)
    this.modelSelected.emit(this.selectedModelId)
  }

  selectModel(model: Model) {
    this.selectedModelId = model.id
    this.showDetails = false
    this.onModelChange()
  }
}
