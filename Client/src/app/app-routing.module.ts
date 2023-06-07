import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { UserCreateComponent } from './components/user-create/user-create.component';
import { BudgetsComponent } from "./components/budgets/budgets.component";
import { AuthGuard } from "./guards/auth.guard";

const routes: Routes = [
  { path: 'user-login', component: UserLoginComponent },
  { path: 'user-create', component: UserCreateComponent },
  { path: 'budgets', component: BudgetsComponent, canActivate: [AuthGuard] },
  { path: '', redirectTo: '/user-login', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
