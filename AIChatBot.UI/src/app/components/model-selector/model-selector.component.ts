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
  selectedModelId: number = 0;
  showDetails = false;

  @Output() modelSelected = new EventEmitter<Model>();

  constructor(private chatService: ChatService) {

  }

  ngOnInit(): void {
    this.getModels()
    const saved = localStorage.getItem('selectedModel')
    this.selectedModelId = saved ? Number(saved) : this.models[0]?.id
    this.modelSelected.emit(this.getModelById(this.selectedModelId))
  }

  getModels() {
    this.chatService.getModels().subscribe(data => {
      this.models = data
      const savedModelId = localStorage.getItem('selectedModel')
      if (this.models.length > 0) {
        if (savedModelId && this.models.some(m => m.id === Number(savedModelId))) {
          this.selectedModelId = Number(savedModelId)
        } else {
          this.selectedModelId = this.models[0].id
        }
        this.modelSelected.emit(this.getModelById(this.selectedModelId))
      } else {
        this.selectedModelId = 0
      }
    })
  }

  onModelChange() {
    localStorage.setItem('selectedModel', this.selectedModelId.toString())
    this.modelSelected.emit(this.getModelById(this.selectedModelId))
  }

  selectModel(model: Model) {
    this.selectedModelId = model.id
    this.showDetails = false
    this.onModelChange()
  }

  private getModelById(modelId: number): Model | undefined {
    return this.models.find(model => Number(model.id) === Number(modelId))
  }
}
