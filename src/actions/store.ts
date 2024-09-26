import { configureStore,Tuple } from "@reduxjs/toolkit";
import {useDispatch} from "react-redux"
import { thunk } from "redux-thunk";
import logger from "redux-logger"
import {DAccount, DLoans, DUser} from "../reducers"
// import { apiSlice } from "./apiSlice";
// import authReducer from "./authSlice"
export const store = configureStore({
  reducer:{
    DUser,
    DAccount,
    DLoans,
    // auth:authReducer,
    // [apiSlice.reducerPath]:apiSlice.reducer
    },
   middleware: () => new Tuple(thunk,logger)

  ,
  devTools:true,

})

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
export const useAppDispatch = useDispatch.withTypes<AppDispatch>() 