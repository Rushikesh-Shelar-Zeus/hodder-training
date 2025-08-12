import { useParams } from "react-router-dom";
import { gql, useQuery } from "@apollo/client";

const GET_ORDERS_DETAILS = gql`
    query($id: String!) {
        orderById(id: $id){
            id
            createdAt
            customerName
            orderItems{
                pizzaName
                quantity
                isGlutenFree
                unitPrice
                subTotal
            }
            totalAmount
        }
}
`;

import "./css/order-details.css";


export default function OrderDetails() {
    const { id } = useParams<{ id: string }>();
    const { loading, error, data } = useQuery(GET_ORDERS_DETAILS, {
        variables: { id },
        fetchPolicy: "cache-and-network",
    });

    if (loading) return (
        <div className="order-details">
            <p>Loading order details...</p>
        </div>
    );

    if (error) return (
        <div className="order-details">
            <p>Error: {error.message}</p>
        </div>
    );

    const order = data?.orderById;

    if (!order) return (
        <div className="order-details">
            <p>No order found.</p>
        </div>
    );

    // Format date nicely
    const formatDate = (dateString: any) => {
        const date = new Date(dateString);
        return date.toLocaleDateString('en-US', {
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    };

    return (
        <div className="order-details">
            {/* Order Header */}
            <div className="order-header">
                <h2>Order Details</h2>
                <div className="order-info">
                    <div className="order-info-item">
                        <label>Order ID</label>
                        <span>#{order.id}</span>
                    </div>
                    <div className="order-info-item">
                        <label>Order Date</label>
                        <span>{formatDate(order.createdAt)}</span>
                    </div>
                </div>
            </div>

            {/* Order Items Table */}
            <div className="order-table-container">
                <table>
                    <thead>
                        <tr>
                            <th>Pizza Name</th>
                            <th>Gluten Free</th>
                            <th>Quantity</th>
                            <th>Unit Price</th>
                            <th>Subtotal</th>
                        </tr>
                    </thead>
                    <tbody>
                        {order.orderItems.map((item: any, index: number) => (
                            <tr key={index}>
                                <td data-label="Pizza Name">{item.pizzaName}</td>
                                <td data-label="Gluten Free">
                                    <span className={item.isGlutenFree ? "gluten-free-yes" : "gluten-free-no"}>
                                        {item.isGlutenFree ? "Yes" : "No"}
                                    </span>
                                </td>
                                <td data-label="Quantity">{item.quantity}</td>
                                <td data-label="Unit Price">₹ {item.unitPrice.toFixed(2)}</td>
                                <td data-label="Subtotal">₹ {item.subTotal.toFixed(2)}</td>
                            </tr>
                        ))}
                        <tr>
                            <td colSpan={4} data-label="">Total Amount</td>
                            <td data-label="Total Amount">₹ {order.totalAmount.toFixed(2)}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    );
}