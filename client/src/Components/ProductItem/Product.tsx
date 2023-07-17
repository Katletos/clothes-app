import { Button } from "@mui/material";
import{ Product } from '../../Types';
import {Wrapper} from './Product.style';
import Logo from './logo512.png';
import { NavLink } from "react-router-dom";
import React from "react";

type Props = {
    item: Product;
    handleAddToCart: (clickedItem: Product) => void;
}

const Item: React.FC<Props> = ({item, handleAddToCart}) => (
    <Wrapper>
        <NavLink to={`/products/${item.id}`}>
            <img src={Logo} alt={item.name} />
        </NavLink>
        <div>
            <h3>{item.name}</h3>
            <h3>${item.price}</h3>
        </div>
        <Button onClick={() => handleAddToCart(item)}>Add to cart</Button>
    </Wrapper>
);

export default Item;