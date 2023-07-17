export type Product = {
  id: number;
  brandId: number;
  categoryId: number;
  name: string;
  price: number;
  quantity: number;
  createdAt: string;
}

export type CartItemType = {
  "productId": number;
  "userId": number;
  "quantity": number;
  "product": Product;
}
// export class LoginUserDto {
//   constructor(email: string, password: string) {
//     this.email = email;
//     this.password = password;
//   }
//
//   email: string;
//   password: string;
// }