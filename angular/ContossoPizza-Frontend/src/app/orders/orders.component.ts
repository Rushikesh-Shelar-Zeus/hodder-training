import { Component, OnInit } from '@angular/core';
import { OrdersService } from '../orders.service';
import { Order } from '../shared/models/Order';
import { PagedResult } from '../shared/models/Pizza';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {
  orders: Order[] = [];
  totalCount: number = 0;
  pageSize: number = 10;
  pageNumber: number = 1;
  sortBy: string = 'Date';
  sortDirection: string = "ASC";


  constructor(private ordersService: OrdersService) { }

  ngOnInit(): void {
    this.loadOrders(this.pageNumber, this.pageSize);
  }

  loadOrders(pageNumber: number, pageSize: number): void {
    this.ordersService.getOrders(pageNumber, pageSize, this.sortBy, this.sortDirection)
      .subscribe((response: PagedResult<Order>) => {
        this.pageNumber = response.pageNumber;
        this.totalCount = response.totalCount;
        this.orders = response.items;
        this.pageSize = response.pageSize
      })
  }

  loadNextPage(): void {
    if ((this.pageNumber * this.pageSize) < this.totalCount) {
      this.pageNumber++;
      this.loadOrders(this.pageNumber, this.pageSize);
    }
  }

  loadPreviousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadOrders(this.pageNumber, this.pageSize);
    }
  }

  loadSorted(sortBy: string, sortDirection: string) {
    this.sortBy = sortBy;
    this.sortDirection = sortDirection;
    this.pageNumber = 1;
    this.loadOrders(this.pageNumber, this.pageSize);
  }

  totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}
