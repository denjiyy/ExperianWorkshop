import React from 'react'
import {connect} from "react-redux"
import * as _actions from "../actions/Duser"
import { DUserState } from '../types'



const DUserForm = (props:any) => {
  return (
    <div>DUserForm</div>
  )
}
const mapStateToProps = (state:DUserState) =>{
  return({
    DUserList:state.list
})
}
const mapActionsToProps={
  createDUser : _actions.create,
  updateDUSer:_actions.update,
}
export default connect(mapStateToProps,mapActionsToProps)(DUserForm)
