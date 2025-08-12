import { useQuery } from "@apollo/client";
import { GET_ORDERS } from "../graphql/queries/orders";

interface Order {
    id: string;
    customerName: string;
    totalAmount: number;
    createdAt: string;
    orderItems?: {
        pizzaName: string;
        quantity: number;
        unitPrice: number;
        subtotal: number;
    }[];
}


import "./css/order.css";
import { useState } from "react";
import { NavLink } from "react-router-dom";

export default function Orders() {

    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [sortBy, setSortBy] = useState("CREATED_AT");
    const [sortOrder, setSortOrder] = useState("ASC");

    const { loading, error, data, refetch } = useQuery(GET_ORDERS, {
        variables: {
            input: {
                pageNumber,
                pageSize,
                sortBy,
                order: sortOrder
            }
        },
        fetchPolicy: "cache-and-network",
    });

    const orders = data?.orders || { items: [], totalCount: 0, pageNumber: 1, pageSize: 10 };
    const totalCount = orders.totalCount;
    const totalPages = Math.ceil(totalCount / pageSize);

    const handleSortChanege = (feild: any, direction: any) => {
        setSortBy(feild);
        setSortOrder(direction);
        setPageNumber(1);
        refetch({
            input: {
                pageNumber: 1,
                pageSize,
                sortBy: feild,
                order: direction
            }
        });
    }

    const handlePageChange = (newPageNumber: number) => {
        setPageNumber(newPageNumber);
        refetch({
            input: {
                pageNumber: newPageNumber,
                pageSize,
                sortBy,
                order: sortOrder
            }
        });
    }

    if (loading) return <p>Loading pizzas...</p>;
    if (error) return <p>Error: {error.message}</p>;


    return (
        <div className="orders-container">
            <h2>Orders</h2>
            <div className="controls-bar">
                <div className="sort-controls">
                    <select
                        value={sortBy}
                        onChange={(e) => handleSortChanege(e.target.value, sortOrder)}
                    >
                        <option value="CREATED_AT">Date</option>
                        <option value="CUSTOMER_NAME">Customer Name</option>
                        <option value="TOTAL_AMOUNT">Total Amount</option>
                    </select>

                    <select
                        value={sortOrder}
                        onChange={(e) => handleSortChanege(sortBy, e.target.value)}
                    >
                        <option value="ASC">Ascending</option>
                        <option value="DESC">Descending</option>
                    </select>
                </div>

                <div className="pagination-controls">
                    <button
                        disabled={pageNumber === 1}
                        onClick={() => handlePageChange(pageNumber - 1)}
                    >Previous</button>
                    <span>Page {pageNumber} / {totalPages || 1}</span>
                    <button
                        disabled={pageNumber >= totalPages}
                        onClick={() => handlePageChange(pageNumber + 1)}

                    > Next</button >
                </div >
            </div >
            <table>
                <thead>
                    <tr>
                        <th>Customer Name</th>
                        <th>Order Date</th>
                        <th>Order Value (₹)</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {orders.items.map((order: Order) => (
                        <tr key={order.id}>
                            <td>{order.customerName}</td>
                            <td>{new Date(order.createdAt).toLocaleDateString()}</td>
                            <td>{order.totalAmount}</td>
                            <NavLink to={`/orders/${order.id}`}>
                                <td>
                                    <button className="view-details-button">View Details</button>
                                </td>
                            </NavLink>
                        </tr>
                    ))}
                </tbody>
            </table >
        </div >
    );
}