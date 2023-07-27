import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'

import { ProductPage } from './Pages/Product/Product';
import { ShopPage } from './Pages/Shop/Shop';
import {LoginPage} from "./Pages/Login/Login";

export const AppRoutes = () => {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<ShopPage />} />
                <Route path="/products/:productId" element={<ProductPage />} />
                <Route path="/login" element={<LoginPage />} />
            </Routes>
        </Router>
    )
}