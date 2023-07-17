import { Product, CartItemType } from "../../Types";
import { useState, useEffect } from "react";
import { useQuery } from "react-query";
import { Drawer, LinearProgress, Grid, Badge } from "@mui/material";
import AddShoppingCartIcon from '@mui/icons-material/AddShoppingCart';
import { Wrapper, StyledButton } from "./Shop.styles";
import Cart  from "../../Components/Cart/Cart";
import ShopItem from "../../Components/ProductItem/Product";
import axios from '../../axiosConfig';
import {useBeforeUnload, useNavigate} from "react-router-dom";
import {LogLevel} from "@microsoft/signalr";
import * as signalR from "@microsoft/signalr";

export const ShopPage = () => {
    const navigate = useNavigate();
    if (localStorage.getItem("user") === null){
        navigate("/login");
    }

    const token = JSON.parse(localStorage.getItem("user") || "").token;
    const userId = JSON.parse(localStorage.getItem("user") || "").userId;

    const [cartOpen, setCartOpen] = useState(false);
    const [connection, setConnection] = useState<null | signalR.HubConnection>(null);

    const handleAddToCart = async (clickedItem: Product) => {
        try {
            await axios.post(
                `http://localhost:5103/api/cart`,
                {
                    quantity: 1,
                    productId: clickedItem.id,
                    userId: userId,
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    };

    const handleRemoveFromCart = async (id: number) => {
        try {
            await axios.put(
                `http://localhost:5103/api/cart`,
                {
                    quantity: 1,
                    productId: id,
                    userId: userId,
                },
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    };

    const getTotalItems = (items: CartItemType[]) =>
        items.reduce((ack: number, items) => ack + items.quantity, 0);

    const handleDropItemFromCart = async (id: number) => {
        try {
            await axios.delete(
                `http://localhost:5103/api/cart`,
                {
                    data: {
                        quantity: 0,
                        productId: id,
                        userId: userId,
                    },
                    headers: {
                        Authorization: `Bearer ${token}`,
                    }
                }
            );
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    };

    useBeforeUnload(() => {
        connection?.invoke(
            "LeaveCart",
            cartItems.map((item) => item.productId),
            userId
        )
        .catch(function (err) {
            return console.error(err.toString());
        });

        connection?.stop()
            .catch(function (err) {
                return console.error(err.toString());
            });
    });

    useEffect(() => {
        if (connection) return;

        try {
            getCartItems();

            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl("http://localhost:5103/products")
                .configureLogging(LogLevel.Information)
                .withAutomaticReconnect()
                .build();

            hubConnection.on("UpdateCart", async () => {
                await getCartItems();
            });

            hubConnection.start()
                .then(() => {
                    hubConnection.invoke(
                        "EnterCart",
                        cartItems.map((item) => item.productId),
                        userId
                    )
                    .catch(function (err) {
                        return console.error(err.toString());
                    });
                    // hubConnection.invoke("GetReservedQuantity", productId)
                    //     .catch(function (err) {
                    //         return console.error(err.toString());
                    //     });
                    // hubConnection.invoke("GetAvailableQuantity", productId)
                    //     .catch(function (err) {
                    //         return console.error(err.toString());
                    //     });
                })
                .catch((error) => console.log(error));

            setConnection(hubConnection);
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    }, []);

    const getCartItems = async () => {
        try {
            const response = await axios.get(
                `http://localhost:5103/api/cart/${userId}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );
            console.log(response.data);
            setCartItems(() => {
                return response.data;
            });
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    };

    const [cartItems, setCartItems] = useState([] as CartItemType[]);

    const getProducts = async (): Promise<Product[]> =>
        await (await fetch('http://localhost:5103/api/products?sectionId=2&categoryId=3')).json();

    const {data, isLoading, error} = useQuery<Product[]>(
        'products',
        getProducts);

    if (isLoading) return <LinearProgress />;
    if (error) return <div>Something went wrong</div>

    return(
        <Wrapper>
            <Drawer anchor='right' open={cartOpen} onClose={() => setCartOpen(false)}>
                <Cart
                    cartItems={cartItems}
                    addToCart={handleAddToCart}
                    removeFromCart={handleRemoveFromCart}
                    deleteItemFromCart={handleDropItemFromCart}
                />
            </Drawer>
            <StyledButton onClick={() => setCartOpen(true)}>
                <Badge badgeContent={getTotalItems(cartItems)} color='error'>
                    <AddShoppingCartIcon/>
                </Badge>
            </StyledButton>
            <Grid container spacing={3}>
                {data?.map(item => ( 
                <Grid item key={item.id}>
                    <ShopItem item={item} handleAddToCart={handleAddToCart} />
                </Grid>))}
            </Grid>
        </Wrapper>
    );
}