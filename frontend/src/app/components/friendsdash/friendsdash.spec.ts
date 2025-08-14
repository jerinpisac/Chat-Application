import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Friendsdash } from './friendsdash';

describe('Friendsdash', () => {
  let component: Friendsdash;
  let fixture: ComponentFixture<Friendsdash>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Friendsdash]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Friendsdash);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
