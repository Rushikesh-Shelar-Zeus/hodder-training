import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Order, OrderPayload } from './shared/models/Order';
import { PagedResult } from './shared/models/Pizza';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {
  private apiUrl = 'http://localhost:5044/api/order';

  constructor(private http: HttpClient) { }
  getOrders(pageNumber: number = 1,
    pageSize: number = 10,
    sortBy: string = "Date",
    sortDirection: string = "ASC"): Observable<PagedResult<Order>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('sortBy', sortBy)
      .set('sortDirection', sortDirection);

    return this.http.get<PagedResult<Order>>(this.apiUrl, { params });
  }

  placeOrder(payload: OrderPayload): Observable<any>{
    return this.http.post(this.apiUrl, payload);
  }
}
