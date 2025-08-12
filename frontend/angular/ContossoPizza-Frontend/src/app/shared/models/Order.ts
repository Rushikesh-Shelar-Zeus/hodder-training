export interface Order {
    id: number;
    customerName?: string;
    orderDate: string;
    totalAmount: number;
}

export interface OrderItem {
    pizzaId: number;
    quantity: number;
}

export interface OrderPayload {
    customerId: number;
    orderItems: OrderItem[];
}
