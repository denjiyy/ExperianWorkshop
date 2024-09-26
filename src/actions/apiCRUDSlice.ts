 import axios,{AxiosResponse} from "axios";
import { LoginProps, SignUpProps } from "../types/form";
import { DUser } from "../reducers";
import { create } from "domain";
import { json } from "stream/consumers";
import { loginUser } from "./Duser";
export const baseURL = "https://localhost:7223/api"
 export interface UrlProps{
    url:string,
    idUser?:string,

 }
axios.defaults.headers.post['Content-Type'] = 'application/json'; 

 export const api={

    Crud({ url , idUser = '' }: UrlProps) {
        return {
          fetchAll: async (): Promise<AxiosResponse<any>> => {
            return await axios.get(url,{
              headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json' // Indicates that the request body is JSON
              },
            });
          },
          fetchById: async (id: string = ''): Promise<AxiosResponse<any>> => {
            return await axios.get(`${url}${id}`,{
              headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json' // Indicates that the request body is JSON
              },
            });
          },
          create: async (newRecord:SignUpProps): Promise<AxiosResponse<any>>=>{
            
            return(axios.post(url,newRecord,{
              headers: {
                      'Accept': 'application/json',
                      'Content-Type': 'application/json' // Indicates that the request body is JSON
                    },
            }))
          },
          loginUser: async (newRecord:LoginProps): Promise<AxiosResponse<any>>=>{
             
            return(axios.post(url,newRecord,{
              headers: {
                      'Accept': 'application/json',
                      'Content-Type': 'application/json' // Indicates that the request body is JSON
                    },
            }))
          },
          update: async (id: string = idUser ?? '', updateRecord: SignUpProps): Promise<AxiosResponse<any>> => {
            return await axios.put(`${url}${id}`, updateRecord,{
              headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json' // Indicates that the request body is JSON
              },
            });
          },
          delete: async (id: string = idUser ?? ''): Promise<AxiosResponse<any>> => {
            return await axios.delete(`${url}${id}`,{
              headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json' // Indicates that the request body is JSON
              },
            });
          },
        };
      },

 }

