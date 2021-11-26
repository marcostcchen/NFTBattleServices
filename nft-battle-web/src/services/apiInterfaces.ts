import { Nft, User } from "../models";

export interface ILoginRes {
  token: string,
  type: string,
  user: User,
}

export interface IBuyNftResponse {
  nft: Nft,
  user: User,
}