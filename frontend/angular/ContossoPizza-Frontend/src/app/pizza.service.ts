import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Pizza, PagedResult } from './shared/models/Pizza';

@Injectable({
  providedIn: 'root'
})
export class PizzaService {
  private apiUrl = 'http://localhost:5044/api/pizza';

  constructor(private http: HttpClient) { }

  getPizzas(
    pageNumber: number = 1,
    pageSize: number = 10,
    sortBy: string = "Price",
    sortDirection: string = "ASC"
  ): Observable<PagedResult<Pizza>> {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString())
      .set('sortBy', sortBy)
      .set('sortDirection', sortDirection);

    return this.http.get<PagedResult<Pizza>>(this.apiUrl, { params });
  }
}
