

import { api } from "./apiCRUDSlice";
import { AppDispatch } from "./store";


interface onCreateProps{
    data:any;
    url?:string;
}
interface onHandleRUDProps extends onCreateProps{
    id:string;
}


export const ACTION_TYPES={
    CREATE: "CREATE",
    UPDATE: "UPDATE",
    DELETE: "DELETE",
    FETCH_ID:"FETCH_ID",
    FETCH_ALL: "FETCH_ALL",
}


export const fetchAll = () =>(dispatch:AppDispatch)=>{
    api.Crud({url:"https://localhost:7223/api/Accounts"}).fetchAll().then((res)=>{
        console.log("API Response:(not dispatched)",res.data);
        dispatch({
            type:ACTION_TYPES.FETCH_ALL,
            payload:res.data,
        });
    }).catch((err:any)=>console.log(err))

}
export const fetchById =({id}:onHandleRUDProps)=>(dispatch:AppDispatch) =>{

    api.Crud({url:"https://localhost:7223/api/Accounts"}).fetchById().then((res)=>{
        console.log("API Response:(not dispatched)",res.data);
        dispatch({
            type:ACTION_TYPES.FETCH_ID,
            payload:res.data,
        })
    }).catch((err:any)=>console.log(err))
}
export const create = ({data}:onCreateProps)=> (dispatch:AppDispatch)=>{
    api.Crud({url:"https://localhost:7223/api/Accounts"}).create(data).then(res=>{
     
     console.log("API Response:",res.data);
     dispatch({
       type:ACTION_TYPES.CREATE,
       payload:res.data
     })
   }).catch(err=>console.log((err)))
 }
 export const update=({id,data}:onHandleRUDProps)=>(dispatch:AppDispatch)=>{
    api.Crud({url:"https://localhost:7223/api/Accounts"}).update(id,data).then(res=>{
        console.log("API Response:(not dispatched)",res.data);
        dispatch({
            type:ACTION_TYPES.UPDATE,
            payload:{id:id,...data}
        })
    }).catch((err:any)=>console.log(err))
 }
 export const delete_ = ({id}:onHandleRUDProps)=>(dispatch:AppDispatch)=>{
    api.Crud({url:"https://localhost:7223/api/Accounts"}).delete(id).then(res=>{
        console.log("API Response:(not dispatched)",res.data);
        dispatch({
            type:ACTION_TYPES.DELETE,
            payload:id
        })
    })
 }