export interface Pizza {
    name: string,
    isGlutenFree: boolean,
    price: number
    id: number,
}


export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}