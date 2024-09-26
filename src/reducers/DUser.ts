import {ACTION_TYPES } from "../actions/Duser"
import { DUserAction, DUserState } from "../types";

interface initialStateProps{
    list?:DUserState[]
}
const initialState:initialStateProps = {
    list:[]
}
export const DUser = (
    state: initialStateProps = initialState, 
    action: DUserAction
): initialStateProps => {
    switch (action.type) {
        case ACTION_TYPES.CREATE:
            return {
                ...state,
                list: action.payload ? [action.payload] : []
            };

        // Add more cases as needed
        default:
            return state;
    }
}
//action type to be defined