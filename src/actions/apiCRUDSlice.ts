 import axios,{AxiosResponse} from "axios";
import { SignUpProps } from "../types/form";
import { DUser } from "../reducers";
import { create } from "domain";
import { json } from "stream/consumers";
 const baseURL = "https://localhost:7223/api/"
 interface UrlProps{
    url?:string,
    idUser?:string,

 }
axios.defaults.headers.post['Content-Type'] = 'application/json'; 

 export default{

    dUser({ url = baseURL + 'auth/', idUser = '' }: UrlProps) {
        return {
          fetchAll: async (): Promise<AxiosResponse<any>> => {
            return await axios.get(url);
          },
          fetchById: async (id: string = ''): Promise<AxiosResponse<any>> => {
            return await axios.get(`${url}${id}`);
          },
          create: async (newRecord:SignUpProps): Promise<AxiosResponse<any>>=>{
            
            return(axios.post(url,newRecord,{
              headers: {
                      'Accept': 'application/json',
                      'Content-Type': 'application/json' // Indicates that the request body is JSON
                    },
            }))
          },
          //  create:  (newRecord: SignUpProps) => {
          //   return  fetch(url, {
          //     method: 'POST', // Setting method to POST
          //     headers: {
          //       'Accept': 'application/json',
          //       'Content-Type': 'application/json' // Indicates that the request body is JSON
          //     },
          //     body: JSON.stringify(newRecord) // Stringify the newRecord object to send it as JSON
          //   });
          // },
          update: async (id: string = idUser ?? '', updateRecord: SignUpProps): Promise<AxiosResponse<any>> => {
            return await axios.put(`${url}${id}`, updateRecord);
          },
          delete: async (id: string = idUser ?? ''): Promise<AxiosResponse<any>> => {
            return await axios.delete(`${url}${id}`);
          },
        };
      },

 }
