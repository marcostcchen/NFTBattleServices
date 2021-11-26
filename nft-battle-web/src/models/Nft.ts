import { Owner } from "./Owner";

export interface Nft {
    id: string,
    name: string,
    type: string,
    health: number,
    attack: number,
    defence: number,
    owner: Owner,
    imageUrl: string
}