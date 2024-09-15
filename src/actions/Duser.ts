import { AppDispatch } from "./store";
import api from "./api";
import { SignUpProps } from "../types/form";
// export const create = (data:any) =>{
//     return({
//         type:'create',
//         payload:data
// })
// } 
interface onCreateProps{
  data:SignUpProps;
  onSuccess?: () => void; 
}
interface onUpdateProps extends onCreateProps{
  id:string;
}
export const ACTION_TYPES = {
  CREATE: "CREATE",
  UPDATE: "UPDATE",
  DELETE: "DELETE",
  FETCH_ALL: "FETCH_ALL",
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
export const create = ({data,onSuccess}:onCreateProps)=>  (dispatch: AppDispatch) =>{
// data = FormateDate(data)
api.dUser({}).create(data).then(async(res)=>{
  const result =await res.json()
  dispatch({
    type:ACTION_TYPES.CREATE,
    payload:result
  })
  // onSuccess()
}).catch(err=>console.log(err))
}
export const update = ({id,data,onSuccess}:onUpdateProps)=>  (dispatch: AppDispatch) =>{
  // data = FormateDate(data)
  api.dUser({}).update(id,data).then(res=>{
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

