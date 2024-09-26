import {ACTION_TYPES } from "../actions/Duser"
import { DAccountState, DAccountAction } from "../types";

interface initialStateProps{
    list?:DAccountState[]//have to include 
    //id of the account as well as id of the loan
}
const initialState:initialStateProps = {
    list:[]
}
export const DLoans = (
    state: initialStateProps = initialState, 
    action: DAccountAction // -> upadte props
): initialStateProps => {
    switch (action.type) {
        case ACTION_TYPES.FETCH_ALL:
            return{
                ...state,
                list: action.payload ? [action.payload] : []
            }
            break;
            case ACTION_TYPES.FETCH_ID:
                return{
                    ...state,
                    list: action.payload ? [action.payload] : []

                }
                break;
        case ACTION_TYPES.CREATE:
            return {
                ...state,
                list: action.payload ? [action.payload] : []
            };
            break;
            case ACTION_TYPES.UPDATE:
                return{
                    ...state,
                    list:state.list?.map(x=>x.id==action.payload?.id? action.payload : x) //returns list of the updated element as well as the others
                    
                }
                break;
                case ACTION_TYPES.DELETE:
                    return{
                        ...state,
                        list:state.list?.filter(x=>x.id !=action.payload?.id) //returns the rest of the elemenets
                    }
        // Add more cases as needed
        default:
            return state;
    }
}
//action type to be defined