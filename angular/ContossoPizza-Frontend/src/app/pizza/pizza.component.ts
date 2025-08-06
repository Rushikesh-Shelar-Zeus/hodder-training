import { Component, OnInit } from '@angular/core';
import { PizzaService, Pizza } from '../pizza.service';

@Component({
  selector: 'app-pizza',
  templateUrl: './pizza.component.html',
  styleUrls: ['./pizza.component.scss']
})
export class PizzaComponent implements OnInit {
  pizzas: Pizza[] = [];

  constructor(private pizzaService: PizzaService) { }

  ngOnInit(): void {
    this.pizzaService.getPizzas().subscribe(data => {
      this.pizzas = data;
    });
  }
}
