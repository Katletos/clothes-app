import { useState } from "react";
import { useQuery } from "react-query";

import Item from './Product/Product';
import Cart from './Cart/Cart';
import { Drawer, LinearProgress, Grid, Badge } from "@mui/material";

import { Wrapper, StyledButton } from "./App.styles";

import AddShoppingCartIcon from '@mui/icons-material/AddShoppingCart';
import { click } from "@testing-library/user-event/dist/click";



const getProducts = async (): Promise<Product[]> =>
  await (await fetch('http://localhost:5103/api/products?sectionId=2&categoryId=3')).json();

function App() {
  const {data, isLoading, error} = useQuery<Product[]>(
    'products',
     getProducts);
  
  const [cartOpen, setCartOpen] = useState(false);
  const [cartItems, setCartitems] = useState([] as Product[]);
     
  const handleAddToCart = (clicketItem: Product) => {
    setCartitems(prev => {
      const isItemInCart = prev.find(item => item.id === clicketItem.id);

      if (isItemInCart){
        return prev.map(item => 
          item.id === clicketItem.id
            ? { ...item, quantity: item.quantity + 1}
            : item
        );
      }

      return [...prev, {...clicketItem, quantity: 1}];
    });
  };
  
  const getTotalItems = (items: Product[]) =>
    items.reduce((ack: number, items) => ack + items.quantity, 0);

  const handleRemoveFromCart = (id: number) => {
    setCartitems(prev =>
      prev.reduce((ack, item) => {
        if (item.id === id){
          if (item.quantity === 1){
            return ack;
          }
          return [...ack, {...item, quantity: item.quantity - 1}];
        } else {
          return [...ack, item];
        }
      }, [] as Product[])
    );
  };
  
  if (isLoading) return <LinearProgress />;
  if (error) return <div>Something went wrong</div>
  

  return (  
    <Wrapper>
      <Drawer anchor='right' open={cartOpen} onClose={() => setCartOpen(false)}>
        <Cart cartItems={cartItems} addToCart={handleAddToCart} removeFromCart={handleRemoveFromCart} />
      </Drawer>
      <StyledButton onClick={() => setCartOpen(true)}>
        <Badge badgeContent={getTotalItems(cartItems)} color='error'>
          <AddShoppingCartIcon/>
        </Badge>
      </StyledButton>
      <Grid container spacing={3}>
        {data?.map(item => ( 
          <Grid item key={item.id}>
            <Item item={item} handleAddToCart={handleAddToCart} />
          </Grid>
        ))}
      </Grid>
    </Wrapper>
  );
}

export default App;
