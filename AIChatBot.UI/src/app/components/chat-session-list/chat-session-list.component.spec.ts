import { TestBed } from '@angular/core/testing'
import { ChatSessionListComponent } from './chat-session-list.component'

describe('ChatSessionListComponent', () => {
    let component: ChatSessionListComponent

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [ChatSessionListComponent]
        })
        const fixture = TestBed.createComponent(ChatSessionListComponent)
        component = fixture.componentInstance
    })

    it('should create', () => {
        expect(component).toBeTruthy()
    })
})
