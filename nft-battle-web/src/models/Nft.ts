import { Owner } from "./Owner";

export interface Nft {
    id: string,
    token_id: string,
    name: string,
    health: number,
    attack: number,
    defence: number,
    owner: Owner,
    image_url: string,
    permaLink: string,
}