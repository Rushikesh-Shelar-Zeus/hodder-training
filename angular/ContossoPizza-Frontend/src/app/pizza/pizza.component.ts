import { Component, OnInit } from '@angular/core';
import { PizzaService } from '../pizza.service';
import { PagedResult, Pizza } from '../shared/models/Pizza';
import { CartService } from '../cart.service';

@Component({
  selector: 'app-pizza',
  templateUrl: './pizza.component.html',
  styleUrls: ['./pizza.component.scss']
})
export class PizzaComponent implements OnInit {
  pizzas: Pizza[] = [];
  totalCount: number = 0;
  pageSize: number = 10;
  pageNumber: number = 1;
  sortBy: string = 'Price';
  sortDirection: string = "ASC";

  constructor(private pizzaService: PizzaService, private cartService: CartService) { }

  ngOnInit(): void {
    this.loadPizzas(this.pageNumber, this.pageSize);
  }

  loadPizzas(pageNumber: number, pageSize: number): void {
    this.pizzaService.getPizzas(pageNumber, pageSize, this.sortBy, this.sortDirection)
      .subscribe((response: PagedResult<Pizza>) => {
        this.pageNumber = response.pageNumber;
        this.totalCount = response.totalCount;
        this.pizzas = response.items;
        this.pageSize = response.pageSize
      })
  }

  loadNextPage(): void {
    if ((this.pageNumber * this.pageSize) < this.totalCount) {
      this.pageNumber++;
      this.loadPizzas(this.pageNumber, this.pageSize);
    }
  }

  loadPreviousPage(): void {
    if (this.pageNumber > 1) {
      this.pageNumber--;
      this.loadPizzas(this.pageNumber, this.pageSize);
    }
  }

  loadSorted(sortBy: string, sortDirection: string): void {
    this.sortBy = sortBy;
    this.sortDirection = sortDirection;
    this.pageNumber = 1;
    this.loadPizzas(this.pageNumber, this.pageSize);
  }

  totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  addToCart(pizza: any): void {
    this.cartService.addToCart(pizza);
    alert(`${pizza.name} added to cart!`);
  }
}
