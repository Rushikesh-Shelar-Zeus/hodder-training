export interface CartItem {
  id: number;
  name: string;
  price: number;
  isGlutenFree: boolean;
  quantity: number;
  subtotal: number;
}