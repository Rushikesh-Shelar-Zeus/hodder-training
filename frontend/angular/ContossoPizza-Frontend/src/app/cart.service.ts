import { Injectable } from '@angular/core';
import { CartItem } from './shared/models/Cart';
import { Pizza } from './shared/models/Pizza';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cart: CartItem[] = [];

  constructor() {
    this.loadCart();
  }

  getCart(): CartItem[] {
    return this.cart;
  }

  addToCart(item: Pizza): void {
    const existingPizza = this.cart.find(ci => ci.id === item.id);

    if (existingPizza) {
      existingPizza.quantity += 1;
      existingPizza.subtotal = Number((existingPizza.quantity * existingPizza.price).toFixed(2));
    } else {
      this.cart.push({
        ...item,
        quantity: 1,
        subtotal: item.price,
      })
    }
    this.saveCart();
  }

  removeFromCart(id: number): void {
    this.cart = this.cart.filter(item => item.id !== id);
    this.saveCart();
  }


  clearCart(): void {
    this.cart = [];
    localStorage.removeItem('cart');
  }

  increaseQuantity(item: CartItem) {
    const cartItem = this.getItem(item.id);
    cartItem.quantity += 1;
  }

  decreaseQuantity(item: CartItem) {
    const cartItem = this.getItem(item.id);
    if (cartItem.quantity > 1) {
      cartItem.quantity -= 1;
    } else {
      this.removeFromCart(cartItem.id)
    }
    this.saveCart()
  }

  private getItem(id: number): CartItem {
    const cartItem = this.cart.find(ci => ci.id === id);
    if (cartItem) {
      return cartItem;
    } else {
      throw new Error('Cart Item Not Found');
    }
  }

  private saveCart(): void {
    localStorage.setItem('cart', JSON.stringify(this.cart));
  }

  getTotalAmount(): number {
    return Number(this.cart.reduce((sum, item) => sum + item.subtotal, 0).toFixed(2));
  }

  private loadCart(): void {
    const data = localStorage.getItem('cart');
    if (data) {
      this.cart = JSON.parse(data);
    }
  }
}
