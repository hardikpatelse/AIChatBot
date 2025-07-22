import { Component, Input } from '@angular/core'
import { User } from '../../services/user.service'

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css'],
    standalone: false
})
export class HeaderComponent {
    @Input() userName!: string
}
