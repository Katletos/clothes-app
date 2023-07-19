import { Button } from "@mui/material";
import{ Product } from '../../Types';
import {Wrapper} from './ProductOnOwnPage.style';
import Logo from '../../Components/ProductItem/logo512.png';
import React from "react";

type Props = {
    item: Product;
    productViews: number;
    reserved: number;
    available: number;
    handleAddToCart: (clickedItem: Product) => void;
}

const ProductOnOwnPage: React.FC<Props> = ({item, handleAddToCart, productViews, reserved, available}) => (
    <Wrapper>
        <div className="row">
            <div><div>
                <h3>{item.name}</h3>
                <h3>${item.price}</h3>
            </div>
                <div>
                    <p>views: {productViews}</p>
                    <p>Reserved: {reserved}</p>
                    <p>Available: {available}</p>
                </div>
            </div>
            <img src={Logo} alt={item.name} />
        </div>
        <Button
            size="small"
            disableElevation
            variant="contained"
            onClick={() => handleAddToCart(item)}
        >
            Add to cart
        </Button>
    </Wrapper>
);

export default ProductOnOwnPage;