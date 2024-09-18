import {ACTION_TYPES } from "../actions/Duser"
import { DUserAction, DUserState } from "../types";

const initialState:DUserState = {
    list:[]
}
export const DUser = (state:DUserState=initialState,action:DUserAction) =>{
    switch(action.type){
        case ACTION_TYPES.FETCH_ALL:
            return{
                ...state,
                list:[...action.payload]
            }
            case ACTION_TYPES.CREATE:
                return{
                    ...state,
                    list:[action.payload]
                }
                case ACTION_TYPES.UPDATE:
                    return{
                        ...state,
                        list:state.list.map(x=>x.id==action.payload.id?action.payload:x)
                    }
                    case ACTION_TYPES.DELETE:
                        return{
                            ...state,
                            list:state.list.filter(x=>x.id!=action.payload)
                        }
            default:
                return state;
    }
}
//action type to be defined