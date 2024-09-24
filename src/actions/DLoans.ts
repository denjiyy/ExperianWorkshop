import { AppDispatch } from "./store";
import api from "./apiCRUDSlice";
import { LoginProps, SignUpProps } from "../types/form";
// export const create = (data:any) =>{
//     return({
//         type:'create',
//         payload:data
// })
// } 
interface onCreateProps{
  data:SignUpProps;
  onSuccess?: () => void; 
  url?:string;
}
interface onLoginProps{
  data:LoginProps;
  onSuccess?: () => void; 
  url?:string;
}
interface onUpdateProps extends onCreateProps{
  id:string;
}
export const ACTION_TYPES = {
  CREATE: "CREATE",
  UPDATE: "UPDATE",
  DELETE: "DELETE",
  FETCH_ID:"FETCH_ID",
  FETCH_ALL: "FETCH_ALL",
  LOGIN_USER:"LOGIN_USER"
};

// const FormateDate = (data:SignUpProps) =>({
//   ...data,
//   dateOfBirth
// :data.dateOfBirth
// })

export const fetchAll = () => (dispatch: AppDispatch) => {
  api
    .dUser({})
    .fetchAll()
    .then((response) => {
      dispatch({
        type: ACTION_TYPES.FETCH_ALL,
        payload: response.data,
      });
    })
    .catch((err) => console.log(err));
};
export const fetchById = ({id}:onUpdateProps) => (dispatch:AppDispatch)=>{
  api.dUser({}).fetchById(id).then((res)=>{
    dispatch({
      type:ACTION_TYPES.FETCH_ID,
      payload:res.data,
    })
  })
}
// export const create = ({data,onSuccess}:onCreateProps)=>  (dispatch: AppDispatch) =>{
// // data = FormateDate(data)
// window.alert(data)
// const res =  api.dUser({}).create(data).then((res)=>{
//    const result = res.json()
//   dispatch({
//     type:ACTION_TYPES.CREATE,
//     payload:res
//   })
//   // onSuccess()
// }).catch(err=>console.log(err))
// }
export const create = (data:SignUpProps)=> (dispatch:AppDispatch)=>{
   api.dUser({}).create(data).then(res=>{
    console.log("API Response:",res.data);
    dispatch({
      type:ACTION_TYPES.CREATE,
      payload:res.data
    })
  }).catch(err=>console.log((err)))
}
export const loginUser = (data:LoginProps)=> (dispatch:AppDispatch)=>{
  api.dUser({}).loginUser(data).then(res=>{
   console.log("API Response:",res.data);
   dispatch({
     type:ACTION_TYPES.CREATE,
     payload:res.data
   })
 }).catch(err=>console.log((err)))
}
export const update = ({id,data,onSuccess}:onUpdateProps)=>  (dispatch: AppDispatch) =>{
  // data = FormateDate(data)
  api.dUser({url :"https://localhost:7223/api/Users"}).update(id,data).then(res=>{
    dispatch({
      type:ACTION_TYPES.UPDATE,
      payload:{id:id,...data}
    })
    // onSuccess()
  }).catch(err=>console.log(err))
  }
  export const delete_ = ({id,data,onSuccess}:onUpdateProps)=>  (dispatch: AppDispatch) =>{
    // data = FormateDate(data)
    api.dUser({}).delete(id).then(res=>{
      dispatch({
        type:ACTION_TYPES.DELETE,
        payload:id
      })
      // onSuccess()
    }).catch(err=>console.log(err))
    }

