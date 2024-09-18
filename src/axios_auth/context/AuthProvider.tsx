import { createContext,useState,Dispatch,SetStateAction } from "react";

interface LayoutProps  { 
    children: JSX.Element
}
interface AuthProps{
    username:string;
    password:string;
    token:string;
}
interface AuthContextInterface {
    auth: AuthProps
    setAuth: Dispatch<SetStateAction<AuthProps>>
  }
const AuthContext = createContext({});
export const AuthProvider = (props:LayoutProps)=>{
    const [auth,setAuth] = useState({})

    return(
        <AuthContext.Provider value={{auth,setAuth}}>
            {props.children}
        </AuthContext.Provider>
    )
}
export default AuthContext;