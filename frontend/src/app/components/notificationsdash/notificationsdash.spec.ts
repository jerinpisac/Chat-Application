import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Notificationsdash } from './notificationsdash';

describe('Notificationsdash', () => {
  let component: Notificationsdash;
  let fixture: ComponentFixture<Notificationsdash>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Notificationsdash]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Notificationsdash);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
