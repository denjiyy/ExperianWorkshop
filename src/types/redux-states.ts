export  interface DUserState{
        token:string;
        administrator:boolean;
}
export interface DUserAction {
    type: string;
    payload?: DUserState; // Define more specific type if possible
  }
export interface UserLoginState{
    token?:any,
    user?:any;
}





export interface DAccountState{
  id:string
}
export interface DAccountAction {
  type: string;
  payload?: DAccountState; // Define more specific type if possible
}