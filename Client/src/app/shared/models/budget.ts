export interface Budget {
  budgetId: string;
  userId: string;
  name: string;
  incomes: Income[];
  expenses: Expense[];
  sharedToUsersIds : string[];
}

export interface Income {
  incomeId: string;
  amount: number;
  description: string;
  category: string;
}

export interface Expense {
  expenseId: string;
  amount: number;
  description: string;
  category: string;
}
