import { Nft } from "./Nft";

export interface User {
    Id: string,
    Name: string,
    Password: string,
    WalletId: string,
    Nfts: Array<Nft>
}