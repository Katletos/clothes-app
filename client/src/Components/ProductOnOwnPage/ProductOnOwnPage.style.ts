import styled from "styled-components";

export const Wrapper = styled.div`
    margin: auto;
    display: flex;
    justify-content: space-between;
    flex-direction: column;
    max-width: 27%;
    border: 1px solid lightblue;
    border-radius: 20px;
  
    button {
        border-radius: 0 0 20px 20px;
    }

    .row {
        display: flex;
        flex-direction: row;
    }
  
    img {
        max-height: 300px;
        max-width: 300px;
        object-fit: cover;
        border-radius: 20px 20px 0 0;
    }

    div {
        font-family: Arial;
        padding: 1rem;
        height: 100%;
    }
`;