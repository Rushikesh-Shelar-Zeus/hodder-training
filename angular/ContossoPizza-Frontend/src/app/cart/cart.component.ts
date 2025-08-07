import { Component, OnInit } from '@angular/core';
import { CartItem } from '../shared/models/Cart';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = []
  totalAmount: number = 0;

  ngOnInit(): void {
    this.cartItems = [
      { id: 1, name: 'Margherita', price: 200, quantity: 2, subtotal: 400 },
      { id: 2, name: 'Pepperoni', price: 250, quantity: 1, subtotal: 250 }
    ];

    this.updateTotal();
  }


  updateTotal(): void {
    this.totalAmount = this.cartItems.reduce((total, item) => {
      return total + (item.price * item.quantity);
    }, 0);
    this.saveCartToLocalStorage();
  }

  removeItem(id: number): void {
    this.cartItems = this.cartItems.filter(ci => ci.id !== id);
    this.updateTotal();
  }

  saveCartToLocalStorage(): void {
    localStorage.setItem('cart', JSON.stringify(this.cartItems));
  }

  loadCartFromLocalStorage(): void {
    const data = localStorage.getItem('cart');
    if (data) {
      this.cartItems = JSON.parse(data);
    }
  }
}
