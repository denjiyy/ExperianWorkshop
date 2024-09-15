export  interface DUserState{
    list:any[],
}
export interface DUserAction {
    type: string;
    payload?: any; // Define more specific type if possible
  }