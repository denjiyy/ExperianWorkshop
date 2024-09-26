import { UserAuthProps } from "@/src/types/userAuth"
import * as S from "./elements"
import {Routes,Route,Link,Navigate} from "react-router-dom"
export const HomePage = (user:UserAuthProps) =>{
    //can be destructured to {token,isAdministrator}
    if(!user){
        return <Navigate to="/login" replace/>
    }
    return <div>Authorized user!!!</div>
}