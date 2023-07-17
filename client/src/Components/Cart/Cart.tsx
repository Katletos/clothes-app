import CartItem from '../CartItem/CartItem';
import Button from '@mui/material/Button';
import { Wrapper } from './Cart.styles';
import { Product, CartItemType } from '../../Types';
import React from "react";

type Props = {
    cartItems: CartItemType[];
    addToCart: (clickedItem: Product) => void;
    removeFromCart: (id: number) => void;
    deleteItemFromCart: (id: number) => void;
};

const Cart: React.FC<Props> = ({ cartItems,
                                   addToCart,
                                   removeFromCart,
                                   deleteItemFromCart
                                  }) => {
    const calculateTotal = (items: CartItemType[]) =>
        items.reduce((ack: number, item) => ack + item.quantity * item.product.price, 0);

    return (
        <Wrapper>
            <h2>Your Shopping Cart</h2>
            {cartItems.length === 0 ? <p>No items in cart.</p> : null}
            {cartItems.sort((a, b) => a.productId - b.productId).map(item => (
                <CartItem 
                    key={item.productId}
                    item={item}
                    addToCart={addToCart}
                    removeFromCart={removeFromCart}
                    deleteItemFromCart={deleteItemFromCart}
                />
            ))}
            <div>
                <h2>Total: ${calculateTotal(cartItems).toFixed(2)}</h2>
                {cartItems.length !== 0
                    ?   <Button 
                            size="large"
                            disableElevation
                            variant="contained"
                            >
                                submit
                        </Button> 
                    :   null
                }
            </div>
        </Wrapper>
    )
};

export default Cart;