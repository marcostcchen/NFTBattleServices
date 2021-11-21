import axios from "axios"
import { User } from "../models";
import { urlApi } from '../utils';

export const login = async (name: string, password: string): Promise<User> => {
    return new Promise(async (resolve, reject) => {
        const params = {
            name,
            password
        }

        try {
            const { data } = await axios.post<User>(`${urlApi}/login`, params)
            resolve(data);
        } catch (error: any) {
            const { response } = error;
            reject(response.data)
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
            reject(response.data)
        }
    })
}