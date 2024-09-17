import { createSlice } from "@reduxjs/toolkit";
import { RootState } from "./store";
import { DUserAction, DUserState, UserLoginState } from "../types";

const authSlice = createSlice({
    name:'auth',
    initialState: {user:null,token:null},
    reducers:{
        setCredentials: (state,action) =>{
            const {user,accessToken} = action.payload
            state.user = user
            state.token = accessToken
        },
        logOut : (state:UserLoginState,action:DUserAction) =>{
            state.user = null;
            state.token = null;
        }
    },
})

export const {setCredentials,logOut} = authSlice.actions

export default authSlice.reducer

export const selectCurrentUser = (state:any) => state.auth.user
export const selectCurrentToken = (state:any) =>state.auth.token


//state to be defined with type

// ------> 1