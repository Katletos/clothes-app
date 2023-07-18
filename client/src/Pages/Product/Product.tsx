import { Wrapper } from './Product.styles';
import { useParams, useBeforeUnload} from 'react-router-dom';
import {CartItemType, Product} from '../../Types';
import { useState, useEffect } from "react";
import { useQuery } from "react-query";
import * as signalR from '@microsoft/signalr';
import {Badge, Drawer, LinearProgress} from "@mui/material";
import  ProductOnOwnPage from "../../Components/ProductOnOwnPage/ProductOnOwnPage";
import {LogLevel} from "@microsoft/signalr";
import axios from "../../axiosConfig";
import Cart from "../../Components/Cart/Cart";
import {StyledButton} from "../Shop/Shop.styles";
import AddShoppingCartIcon from "@mui/icons-material/AddShoppingCart";

export const ProductPage = () => {
    const [connection, setConnection] = useState<null | signalR.HubConnection>(null);
    const [productViews, getViews] = useState(0);
    const [availableQuantity, getAvailable] = useState(0);
    const [reservedQuantity, getReserved] = useState(0);
    const [cartItems, setCartItems] = useState([] as CartItemType[]);
    const [cartOpen, setCartOpen] = useState(false);

    const params = useParams();
    const productId = Number(params.productId);
    const token = JSON.parse(localStorage.getItem("user") || "").token;
    const userId = JSON.parse(localStorage.getItem("user") || "").userId;

    const handleAddToCart = async (clickedItem: Product) => {
        try {
            await axios.post(
                `http://localhost:5103/api/cart/${userId}/items/${clickedItem.id}`,
                {
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

    const handleUpdateCartItem = async (productId: number, newQuantity: number) => {
        try {
            await axios.put(
                `http://localhost:5103/api/cart/${userId}/items/${productId}?newQuantity=${newQuantity}`,
                {
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

    const handleDeleteItemFromCart = async (productId: number) => {
        try {
            await axios.delete(
                `http://localhost:5103/api/cart/${userId}/items/${productId}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    }
                }
            );
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    };
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

    useEffect(() => {
        if (connection) return;

        try {
            getCartItems();

            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl("http://localhost:5103/products")
                .configureLogging(LogLevel.Information)
                .withAutomaticReconnect()
                .build();

            hubConnection.start()
                .then(() => {
                    hubConnection.invoke("EnterCart",
                        cartItems.map((item) => item.productId), userId)
                        .catch(function (err) {
                            return console.error(err.toString());
                        });
                    hubConnection.invoke("EnterProduct", productId, userId)
                        .catch(function (err) {
                            return console.error(err.toString());
                        });
                    hubConnection.invoke("GetReservedQuantity", productId)
                        .catch(function (err) {
                            return console.error(err.toString());
                        });
                    hubConnection.invoke("GetAvailableQuantity", productId)
                        .catch(function (err) {
                            return console.error(err.toString());
                        });
                })
                .catch((error) => console.log(error));

            hubConnection.on("UpdateCart", async () => {
                await getCartItems();
            });

            hubConnection.on("UpdateProductViews", views => {
                getViews(views);
            })

            hubConnection.on("UpdateReservedQuantity", reserved => {
                getReserved(reserved);
            })

            hubConnection.on("UpdateAvailableQuantity", available => {
                getAvailable(available);
            })

            setConnection(hubConnection);
        } catch (error) {
            console.error(`Error: ${error}`);
        }
    }, []);

    useBeforeUnload(() => {
        connection?.invoke("LeaveCart", cartItems.map((item) => item.productId),userId)
            .catch(function (err) {
                return console.error(err.toString());
            });
        connection?.invoke("LeaveProduct", productId, userId)
            .catch(function (err) {
                return console.error(err.toString());
            });
        connection?.stop()
            .catch(function (err) {
                return console.error(err.toString());
            });
    });

    const getProduct = async (): Promise<Product> =>
        await (await fetch(`http://localhost:5103/api/products/${productId}`)).json();

    const {data, isLoading, error} = useQuery<Product>(
        'product',
        getProduct);

    if (isLoading) return <LinearProgress />;
    if (error) return <div>Something went wrong</div>

    return(
        <Wrapper>
            <Drawer anchor='right' open={cartOpen} onClose={() => setCartOpen(false)}>
                <Cart
                    cartItems={cartItems}
                    updateCartItem={handleUpdateCartItem}
                    deleteItemFromCart={handleDeleteItemFromCart}
                />
            </Drawer>
            <StyledButton onClick={() => setCartOpen(true)}>
                <Badge badgeContent={getTotalItems(cartItems)} color='error'>
                    <AddShoppingCartIcon/>
                </Badge>
            </StyledButton>
          <ProductOnOwnPage
            item={data as Product}
            productViews={productViews}
            available={availableQuantity}
            reserved={reservedQuantity}
            handleAddToCart={handleAddToCart}
          />
        </Wrapper>
    );
}