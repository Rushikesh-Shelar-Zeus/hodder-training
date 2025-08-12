import { useCart } from "../context/cartContext";
import { gql, useMutation } from "@apollo/client";

import "./css/cart.css";

const CREATE_ORDER = gql`
    mutation ($input: CreateOrderInput!) {
        createOrder(input: $input) {
            customerId
        }
    }
`;

interface OrderItems {
    pizzaId: string;
    quantity: number;
}

export function Cart() {

    const { cartItems, removeFromCart, clearCart, getTotalAmount, decreaseQuantity, increaseQuantity } = useCart();

    const [createOrder, { data, error, loading }] = useMutation(CREATE_ORDER);

    const placeOrder = async () => {
        try {
            const orderItems: OrderItems[] = cartItems.map(item => ({
                pizzaId: item.id,
                quantity: item.quantity
            }));

            const data = await createOrder({
                variables: {
                    input: {
                        customerId: "689b2aa22e1e1800c3fae799",
                        orderItems
                    }
                }
            });
            console.log(data)
            clearCart();
        } catch (error) {
            console.error("Error placing order:", error);
        }
    }

    if (cartItems.length === 0) {
        return (
            <div>
                <h1>Your Cart is Empty</h1>
                <p>Add some pizzas to your cart!</p>
            </div>
        );
    }


    if (loading) return <p>Loading....</p>;
    if (error) return <p>Error: {error.message}</p>;

    return (
        <div className="cart">
            <h2>Your Cart</h2>

            {cartItems.map((item) => (
                <div key={item.id} className="cart-item">
                    <div>
                        <strong>{item.name}</strong>
                        <div>
                            <button onClick={() => decreaseQuantity(item.id)}>−</button>
                            <span>{item.quantity}</span>
                            <button onClick={() => increaseQuantity(item.id)}>+</button>
                        </div>
                    </div>
                    <div>
                        Price: ₹{item.price} | Subtotal: ₹{item.subtotal.toFixed(2)}
                    </div>
                    <button onClick={() => removeFromCart(item.id)}>Remove</button>
                </div>
            ))}

            <div className="cart-total">
                <p>Total: ₹{getTotalAmount().toFixed(2)}</p>
                <button onClick={clearCart}>Clear Cart</button>
                <button onClick={() => {
                    placeOrder()
                    alert("Order Placed!")
                }}>Place Order</button>
            </div>
        </div>
    );
}