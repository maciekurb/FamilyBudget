import { Component, Inject } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormArray } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DataService } from "../../data.service";
import {Budget} from "../../shared/models/budget";

@Component({
  selector: 'app-budget-dialog',
  templateUrl: './budget-dialog.component.html',
  styleUrls: ['./budget-dialog.component.css']
})
export class BudgetDialogComponent {
  form: FormGroup;
  incomes: FormArray;
  expenses: FormArray;

  constructor(
    public dialogRef: MatDialogRef<BudgetDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private dataService: DataService
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      incomes: this.fb.array([]),
      expenses: this.fb.array([]),
      sharedToUsersIds : [[]]
    });
    this.incomes = this.form.get('incomes') as FormArray;
    this.expenses = this.form.get('expenses') as FormArray;
  }

  createIncome(): FormGroup {
    return this.fb.group({
      amount: ['', Validators.required],
      category: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  createExpense(): FormGroup {
    return this.fb.group({
      amount: ['', Validators.required],
      category: ['', Validators.required],
      description: ['', Validators.required]
    });
  }

  removeIncome(index: number): void {
    this.incomes.removeAt(index);
  }

  removeExpense(index: number): void {
    this.expenses.removeAt(index);
  }

  addIncome(): void {
    this.incomes.push(this.createIncome());
  }

  addExpense(): void {
    this.expenses.push(this.createExpense());
  }

  onCancel(): void {
    this.dialogRef.close();
  }

  onSubmit(): void {
    if (this.form.invalid) {
      return;
    }

    const budget: Budget = this.form.value;
    console.log(budget);

    // Call the DataService method to send the POST request
    this.dataService.postBudget(budget).subscribe(
      res => {
        this.dialogRef.close();

      },
      err => {
        console.error(err.message);
      }
    );
  }
}
