import { TestBed } from '@angular/core/testing'
import { UserFormComponent } from './user-form.component'

describe('UserFormComponent', () => {
    let component: UserFormComponent

    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [UserFormComponent]
        })
        const fixture = TestBed.createComponent(UserFormComponent)
        component = fixture.componentInstance
    })

    it('should create', () => {
        expect(component).toBeTruthy()
    })
})
