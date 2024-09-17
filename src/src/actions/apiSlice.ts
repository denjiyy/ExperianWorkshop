import {createApi,fetchBaseQuery,FetchArgs,FetchBaseQueryError,BaseQueryFn, BaseQueryApi} from "@reduxjs/toolkit/query/react"
import { setCredentials,logOut } from "./authSlice"
import {RootState} from './store'
// import { BaseQueryApi } from "../types/baseQuery"
const baseQuery = fetchBaseQuery({
    baseUrl:'http://localhost:7223',
    credentials:'include',
    prepareHeaders:(headers,{getState})=>{
        const state = getState() as any
        //const state = getState() as RootState
        
         const token = state.auth.token
        if(token){
            headers.set("Authorization",`Bearer ${token}`) 

        }
        return headers
    }
})

const baseQueryWithReauth: BaseQueryFn<
string | FetchArgs, 
unknown,
FetchBaseQueryError
> = async(args,api,extraOptions)=>{
    let result = await baseQuery(args,api,extraOptions)
    if(result?.error?.status===403){
        console.log('sending refresh token')
        //send refresh token to get new acess token
        const refreshResult = await baseQuery('api/Users/login',api,extraOptions)
        console.log(refreshResult)
        if(refreshResult?.data){
            const state = api.getState() as any
            const user = state.auth.user
            //store the new token
            api.dispatch(setCredentials({...refreshResult.data,user}))
            //possibly have to change the order for the dispatch of the parameters

            //retry original query with new access token
            result = await baseQuery(args,api,extraOptions)
        }else{
            api.dispatch(logOut())
        }
    }
    return result;
}
export const apiSlice = createApi({
    baseQuery:baseQueryWithReauth,
    endpoints : builder =>({}),
})

// credentials will send back http-only cookie

//baseQueryReAUth - wrapper that reattempts to see if an acess token has expired
//refreshResult?.data - the user should already be logged in case we are logged
