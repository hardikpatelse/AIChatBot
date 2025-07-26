import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SessionFilesComponent } from './session-files.component';

describe('SessionFilesComponent', () => {
  let component: SessionFilesComponent;
  let fixture: ComponentFixture<SessionFilesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [SessionFilesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SessionFilesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});