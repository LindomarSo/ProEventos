import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EventosListComponent } from './eventos-list.component';

describe('EventosListComponent', () => {
  let component: EventosListComponent;
  let fixture: ComponentFixture<EventosListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EventosListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EventosListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
