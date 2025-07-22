import { TestBed } from '@angular/core/testing'
import { HeaderComponent } from './header.component'

describe('HeaderComponent', () => {
    let component: HeaderComponent

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [HeaderComponent]
        })
        const fixture = TestBed.createComponent(HeaderComponent)
        component = fixture.componentInstance
    })

    it('should create', () => {
        expect(component).toBeTruthy()
    })
})
