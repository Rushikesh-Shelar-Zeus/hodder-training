import { Component, OnInit } from '@angular/core';
import { CartItem } from '../shared/models/Cart';
import { CartService } from '../cart.service';
import { OrderItem, OrderPayload } from '../shared/models/Order';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.scss']
})
export class CartComponent implements OnInit {
  cartItems: CartItem[] = []
  totalAmount: number = 0;

  constructor(private cartService: CartService, private orderService: OrdersService) { }

  ngOnInit(): void {
    this.cartItems = this.cartService.getCart();
    this.totalAmount = this.cartService.getTotalAmount();
  }

  removeItem(id: number): void {
    this.cartService.removeFromCart(id);
    this.cartItems = this.cartService.getCart();
    this.totalAmount = this.cartService.getTotalAmount();
  }

  increaseQuantity(item: CartItem) {
    this.cartService.increaseQuantity(item);
    this.updateCart();
  }

  decreaseQuantity(item: CartItem) {
    this.cartService.decreaseQuantity(item);
    if (item.quantity === 1) {
      this.removeItem(item.id)
    }
    this.updateCart();
  }

  updateCart() {
    this.cartItems.forEach(item => {
      item.subtotal = Number((item.price * item.quantity).toFixed(2));
    });

    this.totalAmount = this.cartService.getTotalAmount();
  }

  placeOrder(): void {
    //Fake Customer Id (Replace with Authentication Logic)
    const customerId = 1095;

    const orderItems: OrderItem[] = this.cartItems.map(item => ({
      pizzaId: item.id,
      quantity: item.quantity
    }));

    const payload: OrderPayload = {
      customerId,
      orderItems
    };

    this.orderService.placeOrder(payload).subscribe({
      next: (response) => {
        alert("Order Placed SuccessFully");
        this.cartService.clearCart();
        this.cartItems = [];
        this.totalAmount = 0;
      },
      error: (err) => {
        alert('Failed to place order.');
        console.error(err);
      }
    })

  }
}
