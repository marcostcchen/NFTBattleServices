import axios, { AxiosRequestConfig } from "axios"
import { Nft, User } from "../models";
import { TokenConstant, urlApi } from '../utils';
import { IBuyNftResponse, ILoginRes } from "./apiInterfaces";

export const login = async (name: string, password: string): Promise<ILoginRes> => {
  return new Promise(async (resolve, reject) => {
    const params = {
      name,
      password
    }

    try {
      const { data } = await axios.post<ILoginRes>(`${urlApi}/login`, params)
      resolve(data);
    } catch (error: any) {
      const { response } = error;
      reject(response?.data ?? "Erro")
    }
  })
}

export const createUser = async (name: string, password: string, walletId: string): Promise<User> => {
  return new Promise(async (resolve, reject) => {
    const params = {
      name,
      password,
      walletId
    }

    try {
      const { data } = await axios.post<User>(`${urlApi}/user`, params)
      resolve(data);
    } catch (error: any) {
      const { response } = error;
      reject(response?.data ?? "Erro")
    }
  })
}


export const getNfts = async (): Promise<Array<Nft>> => {
  return new Promise(async (resolve, reject) => {
    const config: AxiosRequestConfig<any> = {
      headers: {
        'Authorization': await localStorage.getItem(TokenConstant) ?? "",
        'Content-Type': 'application/json',
      }
    }

    try {
      const { data } = await axios.get<Array<Nft>>(`${urlApi}/nfts`, config)
      resolve(data);
    } catch (error: any) {
      const { response } = error;
      reject(response?.data ?? "Erro")
    }
  })
}


export const getUserNfts = async (): Promise<Array<Nft>> => {
  return new Promise(async (resolve, reject) => {
    const config: AxiosRequestConfig<any> = {
      headers: {
        'Authorization': await localStorage.getItem(TokenConstant) ?? "",
        'Content-Type': 'application/json',
      }
    }

    try {
      const { data } = await axios.get<Array<Nft>>(`${urlApi}/usernft`, config)
      resolve(data);
    } catch (error: any) {
      const { response } = error;
      reject(response.data)
    }
  })
}

export const fetchBuyNft = async (idNft: string): Promise<Nft> => {
  return new Promise(async (resolve, reject) => {
    const params = {
      idNft
    }

    const config: AxiosRequestConfig<any> = {
      headers: {
        'Authorization': await localStorage.getItem(TokenConstant) ?? "",
        'Content-Type': 'application/json',
      }
    }

    try {
      const { data } = await axios.post<IBuyNftResponse>(`${urlApi}/usernft/buynft`, params, config)
      resolve(data.nft);
    } catch (error: any) {
      const { response } = error;
      reject(response.data)
    }
  })
}


export const fetchSellNft = async (idNft: string): Promise<Nft> => {
  return new Promise(async (resolve, reject) => {
    const params = {
      idNft
    }

    const config: AxiosRequestConfig<any> = {
      headers: {
        'Authorization': await localStorage.getItem(TokenConstant) ?? "",
        'Content-Type': 'application/json',
      }
    }

    try {
      const { data } = await axios.post<Nft>(`${urlApi}/usernft/sellnft`, params, config)
      resolve(data);
    } catch (error: any) {
      const { response } = error;
      reject(response.data)
    }
  })
}