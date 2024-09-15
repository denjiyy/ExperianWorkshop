import { configureStore,Tuple } from "@reduxjs/toolkit";
import {useDispatch} from "react-redux"
import { thunk } from "redux-thunk";
import logger from "redux-logger"
import {DUser} from "../reducers"
export type RootState = ReturnType<typeof store.getState>
export const store = configureStore({
  reducer:{DUser
    },
  middleware: () => new Tuple(thunk,logger)
})

export type AppDispatch = typeof store.dispatch;
export const useAppDispatch = useDispatch.withTypes<AppDispatch>() 