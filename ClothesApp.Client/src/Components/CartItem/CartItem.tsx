import Button from '@mui/material/Button';
import { Product, CartItemType } from '../../Types';
import { Wrapper } from './CartItem.styles';
import Logo from './logo512.png';
import React from "react";

type Props = {
    item: CartItemType;
    updateCartItem: (productId: number, newQuantity: number) => void;
    deleteItemFromCart: (productId: number) => void;
}

const CartItem: React.FC<Props> = ({item, updateCartItem, deleteItemFromCart}) => (
    <Wrapper>
        <div>
            <h3>{item.product.name}</h3>
            <div className="column">
                <div className="information">
                    <p>Price: ${item.product.price}</p>
                    <p>Total: ${(item.quantity * item.product.price).toFixed(2)}</p>
                </div>
                <div className="information">
                    <p>Available: {item.product.quantity}</p>
                </div>
            </div>
            <div className="buttons">
                <Button 
                    size="small"
                    disableElevation
                    variant="contained"
                    onClick={() => updateCartItem(item.productId, item.quantity - 1)}
                    >
                        -
                </Button>
                <p>{item.quantity}</p>
                <Button 
                    size="small"
                    disableElevation
                    variant="contained"
                    onClick={() => updateCartItem(item.productId, item.quantity + 1)}
                    >
                        +
                </Button>
                <Button
                    size="small"
                    disableElevation
                    variant="contained"
                    onClick={() => deleteItemFromCart(item.product.id)}
                >
                    X
                </Button>
            </div>
        </div>
        <img src={Logo} alt="photo"></img>
    </Wrapper>
);

export default CartItem;