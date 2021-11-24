import { User } from "../models";

export interface ILoginRes {
  token: string,
  type: string,
  user: User,
}