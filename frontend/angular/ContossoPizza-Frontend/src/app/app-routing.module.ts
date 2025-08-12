import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { PizzaComponent } from './pizza/pizza.component';
import { OrdersComponent } from './orders/orders.component';
import { CartComponent } from './cart/cart.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'pizzas', component: PizzaComponent },
  { path: 'orders', component: OrdersComponent },
  { path: 'cart', component: CartComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
