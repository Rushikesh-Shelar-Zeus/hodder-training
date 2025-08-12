import { useQuery } from "@apollo/client";
import { GET_PIZZAS } from "../graphql/queries/pizza";
import { useCart } from "../context/cartContext";

import "./css/pizza.css";
import { useState } from "react";

export interface Pizza {
    id: string;
    name: string;
    isGlutenFree: boolean;
    price: number;
}

export default function Pizzas() {

    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [sortBy, setSortBy] = useState("PRICE");
    const [sortOrder, setSortOrder] = useState("ASC");

    const { addToCart } = useCart();

    const { loading, error, data, refetch } = useQuery(GET_PIZZAS, {
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

    const pizzas = data?.pizzas || { items: [], totalCount: 0, pageNumber: 1, pageSize: 10 };
    const totalCount = pizzas.totalCount;
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

    const notifyToast = () => {

    }

    if (loading) return <p>Loading pizzas...</p>;
    if (error) return <p>Error: {error.message}</p>;


    return (
        <div className="pizza-container">
            <h2>Pizza Menu</h2>
            <div className="controls-bar">
                <div className="sort-controls">
                    <select
                        value={sortBy}
                        onChange={(e) => handleSortChanege(e.target.value, sortOrder)}
                    >
                        <option value="PRICE">Price</option>
                        <option value="NAME">Name</option>
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
                        <th>Name</th>
                        <th>Gluten Free?</th>
                        <th>Price (₹)</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {pizzas.items.map((pizza: Pizza) => (
                        <tr key={pizza.id}>
                            <td>{pizza.name}</td>
                            <td className={pizza.isGlutenFree ? "gluten-free-yes" : "gluten-free-no"}
                            >{pizza.isGlutenFree ? "Yes" : "No"}</td>
                            <td>{pizza.price}</td>
                            <button
                                className="action-btn"
                                onClick={() => {
                                    addToCart({
                                        id: pizza.id,
                                        name: pizza.name,
                                        price: pizza.price,
                                        isGlutenFree: pizza.isGlutenFree
                                    });
                                    alert(`${pizza.name} added to cart`);
                                }}>
                                Add to Cart
                            </button>
                        </tr>
                    ))}
                </tbody>
            </table >
        </div >
    );
}