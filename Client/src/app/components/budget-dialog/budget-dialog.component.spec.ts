import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BudgetDialogComponent } from './budget-dialog.component';

describe('BudgetDialogComponent', () => {
  let component: BudgetDialogComponent;
  let fixture: ComponentFixture<BudgetDialogComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [BudgetDialogComponent]
    });
    fixture = TestBed.createComponent(BudgetDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
