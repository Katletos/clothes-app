import CartItem from '../CartItem/CartItem';

import Button from '@mui/material/Button';

import { Wrapper } from './Cart.styles';

import { Product } from '../App';

type Props = {
    cartItems: Product[];
    addToCart: (clickedItem: Product) => void;
    removeFromCart: (id: number) => void;
};

const Cart: React.FC<Props> = ({ cartItems, addToCart, removeFromCart}) => {
    const calculateTotal = (items: Product[]) => 
        items.reduce((ack: number, item) => ack + item.quantity * item.price, 0);


    return (
        <Wrapper>
            <h2>Your Shopping Cart</h2>
            {cartItems.length === 0 ? <p>No items in cart.</p> : null}
            {cartItems.map(item => (
                <CartItem 
                    key={item.id}
                    item={item}
                    addToCart={addToCart}
                    removeFromCart={removeFromCart}
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