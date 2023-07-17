import { Button } from "@mui/material";
import{ Product } from '../../Types';
import {Wrapper} from './ProductOnOwnPage.style';
import Logo from '../../Components/ProductItem/logo512.png';

type Props = {
    item: Product;
    productViews: number;
    reserved: number;
    available: number;
}

const ProductOnOwnPage: React.FC<Props> = ({item, productViews, reserved, available}) => (
    <Wrapper>
        <div>name: {item.name}</div>
        <div>price: ${item.price}</div>
        <div>views: {productViews}</div>
        <div>Reserved: {reserved}</div>
        <div>Available: {available}</div>
        <img src={Logo} alt={item.name} />
    </Wrapper>
);

export default ProductOnOwnPage;