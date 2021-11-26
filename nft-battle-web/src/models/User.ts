import { Owner } from "./Owner";

export interface User {
    id: string,
    name: string,
    password: string,
    walletId: string,
    nfts: Array<Owner>
}