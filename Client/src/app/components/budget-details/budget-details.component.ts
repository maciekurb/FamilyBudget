import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DataService } from "../../data.service";
import {Budget} from "../../shared/models/budget";

@Component({
  selector: 'app-budget-details',
  templateUrl: './budget-details.component.html',
  styleUrls: ['./budget-details.component.css']
})

export class BudgetDetailsComponent {
  budget: Budget;
  form: FormGroup;

  constructor(
    public dialogRef: MatDialogRef<BudgetDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private dataService: DataService,
  ) {
    this.budget = data.budget;

    this.form = this.fb.group({
      name: [this.budget.name],
      incomes: this.fb.array([]),
      expenses: this.fb.array([]),
      sharedToUsersIds : [[]]
    });

    this.budget.incomes.forEach(income => {
      this.incomes.push(this.createIncome(income));
    });

    this.budget.expenses.forEach(expense => {
      this.expenses.push(this.createExpense(expense));
    });
  }

  createIncome(income: any): FormGroup {
    return this.fb.group({
      amount: income.amount,
      category: income.category,
      description: income.description
    });
  }

  createExpense(expense: any): FormGroup {
    return this.fb.group({
      amount: expense.amount,
      category: expense.category,
      description: expense.description
    });
  }

  get incomes(): FormArray {
    return this.form.get('incomes') as FormArray;
  }

  get expenses(): FormArray {
    return this.form.get('expenses') as FormArray;
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
