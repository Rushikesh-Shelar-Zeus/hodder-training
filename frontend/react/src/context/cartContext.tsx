import { createContext, useContext, useEffect, useState, type ReactNode } from "react";
import React from "react";

export interface CartItem {
    id: string;
    name: string;
    price: number;
    quantity: number;
    subtotal: number;
    isGlutenFree?: boolean;
}

interface CartContextType {
    cartItems: CartItem[];
    addToCart: (pizza: Omit<CartItem, "quantity" | "subtotal">) => void;
    removeFromCart: (id: string) => void;
    increaseQuantity: (id: string) => void;
    decreaseQuantity: (id: string) => void;
    clearCart: () => void;
    getTotalAmount: () => number;
}

const CartContext = createContext<CartContextType | undefined>(undefined);

export const useCart = (): CartContextType => {
    const context = useContext(CartContext);
    if (!context) {
        throw new Error("useCart must be used within a CartProvider");
    }
    return context;
}

export const CartProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [cartItems, setCartItems] = useState<CartItem[]>(() => {
        const savedCart = localStorage.getItem("cartItems");
        return savedCart ? JSON.parse(savedCart) : [];
    });

    useEffect(() => {
        localStorage.setItem("cartItems", JSON.stringify(cartItems));
    }, [cartItems]);

    const addToCart = (pizza: Omit<CartItem, "quantity" | "subtotal">) => {
        setCartItems((prevItem: CartItem[]) => {
            const existingItem = prevItem.find(item => item.id === pizza.id);
            if (existingItem) {
                return prevItem.map(item =>
                    item.id === pizza.id
                        ? { ...item, quantity: item.quantity + 1, subtotal: (item.quantity + 1) * item.price }
                        : item
                );
            }
            return [...prevItem, { ...pizza, quantity: 1, subtotal: pizza.price }];
        });
    }

    const removeFromCart = (id: string) => {
        setCartItems((prevItems) => prevItems.filter(item => item.id !== id));
    }

    const clearCart = () => {
        setCartItems([]);
    }

    const increaseQuantity = (id: string) => {
        setCartItems((prevItems) => {
            return prevItems.map(item =>
                item.id === id
                    ? { ...item, quantity: item.quantity + 1, subtotal: (item.quantity + 1) * item.price }
                    : item
            );
        });
    }

    const decreaseQuantity = (id: string) => {
        setCartItems((prevItems) => {
            return prevItems.map(item => {
                if (item.id === id) {
                    const newQuantity = item.quantity - 1;
                    if (newQuantity <= 0) {
                        return null;
                    }
                    return { ...item, quantity: newQuantity, subtotal: newQuantity * item.price };
                }
                return item;
            }).filter(item => item !== null);
        });
    }

    const getTotalAmount = () => {
        return cartItems.reduce((total, item) => total + item.subtotal, 0);
    }

    return (
        <CartContext.Provider value={{
            cartItems,
            addToCart,
            removeFromCart,
            increaseQuantity,
            decreaseQuantity,
            clearCart,
            getTotalAmount
        }}>
            {children}
        </CartContext.Provider>
    )
}


