import { Component, Input } from '@angular/core'
import { Model } from '../../entities/model'

@Component({
    selector: 'app-model-details',
    templateUrl: './model-details.component.html',
    styleUrls: ['./model-details.component.css'],
    standalone: false
})
export class ModelDetailsComponent {
    @Input() model: Model | null = null;
}
