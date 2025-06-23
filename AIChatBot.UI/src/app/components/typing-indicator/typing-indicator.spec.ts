import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TypingIndicator } from './typing-indicator';

describe('TypingIndicator', () => {
  let component: TypingIndicator;
  let fixture: ComponentFixture<TypingIndicator>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TypingIndicator]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TypingIndicator);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
