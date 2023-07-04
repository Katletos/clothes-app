import Button from '@mui/material/Button';

import { Product } from '../App';

import {Wrapper} from './CartItem.styles';

type Props = {
    item: Product;
    addToCart: (clickedItem: Product) => void;
    removeFromCart: (id: number) => void;
}

const CartItem: React.FC<Props> = ({item, addToCart, removeFromCart}) => (
    <Wrapper>
        <div>
            <h3>{item.name}</h3>
            <div className="information">
                <p>Price: ${item.price}</p>
                <p>Total: ${(item.quantity * item.price).toFixed(2)}</p>
            </div>
            <div className="buttons">
                <Button 
                    size="small"
                    disableElevation
                    variant="contained"
                    onClick={() => removeFromCart(item.id)}
                    >
                        -
                </Button>
                <p>{item.quantity}</p>
                <Button 
                    size="small"
                    disableElevation
                    variant="contained"
                    onClick={() => addToCart(item)}
                    >
                        +
                </Button>
            </div>
        </div>
        <img src="" alt="photo"></img>
    </Wrapper>
);

export default CartItem;