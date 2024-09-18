import { apiSlice } from "./apiSlice";
import { createEntityAdapter, createSelector } from "@reduxjs/toolkit";

export const authAdapter = createEntityAdapter({})
const initialState = authAdapter.getInitialState()
 
export const authApiSlice = apiSlice.injectEndpoints({
    endpoints:builder=>({
        login:builder.mutation({
            query:credentials=>({
                url:'/api/auth',
                headers:{
                    'Accept': 'application/json',
                    'Content-Type': 'application/json' 
                },
                method:'POST',
                body:{...credentials}
            }),
        })
    })
})
export const {
    useLoginMutation 
}= authApiSlice
// export const selectAuthResult = authApiSlice.endpoints.login.select()
// const selectAuthData = createSelector(selectAuthResult,selectTheData=>selectTheData.data)

//Mutation is for POST,PUT,DELETE
