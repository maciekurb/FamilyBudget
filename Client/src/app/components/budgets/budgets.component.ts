import { Component, OnInit, ChangeDetectorRef  } from '@angular/core';
import { Budget } from '../../shared/models/budget';
import { DataService } from '../../data.service';
import {MatDialog, MatDialogRef} from '@angular/material/dialog';
import { BudgetDialogComponent } from '../budget-dialog/budget-dialog.component';
import {BudgetDetailsComponent} from "../budget-details/budget-details.component";


@Component({
  selector: 'app-budgets',
  templateUrl: './budgets.component.html',
  styleUrls: ['./budgets.component.css']
})
export class BudgetsComponent implements OnInit {
  budgets: Budget[] = [];
  displayedColumns: string[] = ['name', 'detailsButton'];

  constructor(private dataService: DataService, public dialog: MatDialog, private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.loadBudgets();
  }

  loadBudgets(): void {
    this.dataService.getBudgets().subscribe(
      (budgets: Budget[]) => {
        this.budgets = budgets;
        this.cdr.detectChanges();

      },
      (error: any) => {
        console.error(error);
      }
    );
  }

  openDialog(): void {
    const dialogRef: MatDialogRef<BudgetDialogComponent> = this.dialog.open(BudgetDialogComponent, {
      panelClass: 'custom-dialog'
    });

    dialogRef.afterClosed().subscribe((updatedBudget: Budget) => {
      this.loadBudgets();
    });
  }

  openDetails(id : string): void {
    const selectedBudget: Budget | undefined = this.budgets.find(budget => budget.budgetId === id);
    const dialogRef: MatDialogRef<BudgetDetailsComponent> = this.dialog.open(BudgetDetailsComponent, {
      panelClass: 'custom-dialog',
      data: { budget: selectedBudget }
    });
  }
}
