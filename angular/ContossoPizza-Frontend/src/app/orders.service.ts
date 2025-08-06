import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Orders {
  id: number;
  customerName: string;
  orderDate: Date;
  totalAmount: number;
  orderItems: Array<{
    pizzaId: number;
    pizzaName: string;
    quantity: number;
    unitPrice: number;
    totalPrice: number;
  }>;
}

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  private apiUrl = 'http://localhost:5044/api';

  constructor(private http: HttpClient) { }
  getOrders(): Observable<Orders[]> {
    return this.http.get<Orders[]>(`${this.apiUrl}/order`);
  }
}
