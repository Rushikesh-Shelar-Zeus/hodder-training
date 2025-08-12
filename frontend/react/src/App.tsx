import './App.css'
import Home from './components/home'
import Navbar from './components/navbar'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import Pizzas from './components/pizzas'
import Orders from './components/order'
import { Cart } from './components/cart'
import OrderDetails from './components/order-deatils'

function App() {
  return (
    <Router>
      <Navbar projectName="Contoso Pizza" />
      <div className='container'>

        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/pizzas" element={<Pizzas />} />
          <Route path="/orders" element={<Orders />} />
          <Route path="/orders/:id" element={<OrderDetails />} />
          <Route path="/cart" element={<Cart />} />
        </Routes>
      </div>
    </Router>
  )
}

export default App
