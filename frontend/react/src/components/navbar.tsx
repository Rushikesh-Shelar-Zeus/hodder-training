import { useState } from "react";
import { NavLink } from "react-router-dom";

import "./css/navbar.css";

interface NavbarProps {
    projectName: string;
}

export default function Navbar({ projectName }: NavbarProps) {
    const [menuOpen, setMenuOpen] = useState(false);

    const toggleMenu = () => {
        setMenuOpen((prev) => !prev);
    };
    return (
        <header className="navbar" >
            <div className="navbar-inner" >
                <NavLink to="/" className="navbar-logo" >
                    {projectName}
                </NavLink>

                < button
                    className="navbar-toggle"
                    onClick={toggleMenu}
                >
                    <span className="bar" > </span>
                    < span className="bar" > </span>
                    < span className="bar" > </span>
                </button>

                < ul className={`navbar-links ${menuOpen ? "open" : ""}`
                }>
                    <li>
                        <NavLink to="/" end >
                            Home
                        </NavLink>
                    </li>
                    < li >
                        <NavLink to="/pizzas" > Pizzas </NavLink>
                    </li>
                    < li >
                        <NavLink to="/orders" > Orders </NavLink>
                    </li>
                    < li >
                        <NavLink to="/cart" > Cart </NavLink>
                    </li>
                </ul>
            </div>
        </header >
    );
}       