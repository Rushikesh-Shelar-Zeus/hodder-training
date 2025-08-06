import { Component, OnInit } from '@angular/core';
import { OrdersService, Orders } from '../orders.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent {
  orders: Orders[] = [];
  
  constructor(private ordersService: OrdersService) { }

  ngOnInit(): void {
    this.ordersService.getOrders().subscribe(data => {
      this.orders = data;
    });
  }
}
