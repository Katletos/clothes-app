import { Button } from "@mui/material";

import{ Product} from '../App';

import {Wrapper} from './Product.style';

import Logo from './logo512.png';

type Props = {
    item: Product;
    handleAddToCart: (clickedItem: Product) => void;
}

const Item: React.FC<Props> = ({item, handleAddToCart}) => (
    <Wrapper>
        <img src={Logo} alt={item.name} />
        <div>
            <h3>{item.name}</h3>
            <h3>${item.price}</h3>
        </div>
        <Button onClick={() => handleAddToCart(item)}>Add to cart</Button>
    </Wrapper>
);

export default Item;