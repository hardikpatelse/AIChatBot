import { TestBed } from '@angular/core/testing'
import { NewChatSessionComponent } from './new-chat-session.component'

describe('NewChatSessionComponent', () => {
    let component: NewChatSessionComponent

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [NewChatSessionComponent]
        })
        const fixture = TestBed.createComponent(NewChatSessionComponent)
        component = fixture.componentInstance
    })

    it('should create', () => {
        expect(component).toBeTruthy()
    })
})
