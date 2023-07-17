import { Wrapper } from './Product.styles';
import { useParams, useBeforeUnload} from 'react-router-dom';
import { Product } from '../../Types';
import { useState, useEffect } from "react";
import { useQuery } from "react-query";
import * as signalR from '@microsoft/signalr';
import { LinearProgress } from "@mui/material";
import  ProductOnOwnPage from "../../Components/ProductOnOwnPage/ProductOnOwnPage";
import {LogLevel} from "@microsoft/signalr";

export const ProductPage = () => {
    const [connection, setConnection] = useState<null | signalR.HubConnection>(null);
    const [productViews, getViews] = useState(0);
    const [availableQuantity, getAvailable] = useState(0);
    const [reservedQuantity, getReserved] = useState(0);

    const params = useParams();
    const productId = Number(params.productId);
    const token = JSON.parse(localStorage.getItem("user") || "").token;
    const userId = JSON.parse(localStorage.getItem("user") || "").userId;


    useEffect(() => {
        if (connection) return;

        try {
            const hubConnection = new signalR.HubConnectionBuilder()
                .withUrl("http://localhost:5103/products")
                .configureLogging(LogLevel.Information)
                .withAutomaticReconnect()
                .build();

            hubConnection.start()
                .then(() => {
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
        connection?.invoke("LeaveProduct", productId, userId)
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
          <ProductOnOwnPage
            item={data as Product}
            productViews={productViews}
            available={availableQuantity}
            reserved={reservedQuantity} />
        </Wrapper>
    );
}