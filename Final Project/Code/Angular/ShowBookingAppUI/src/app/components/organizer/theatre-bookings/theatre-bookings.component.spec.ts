import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TheatreBookingsComponent } from './theatre-bookings.component';

describe('TheatreBookingsComponent', () => {
  let component: TheatreBookingsComponent;
  let fixture: ComponentFixture<TheatreBookingsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TheatreBookingsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TheatreBookingsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
