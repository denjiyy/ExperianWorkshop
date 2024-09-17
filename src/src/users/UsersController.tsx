import React, { useEffect } from 'react'
import {connect} from "react-redux"
import { DUserState } from '../types'
import * as _actions from "../actions/Duser"
 const DUser = (props:any) => {
  useEffect(()=>{
    props.fetchAllCandidates()
  },[]);
  return (
    <div>DUser</div>
  )
}
const mapStateToProps = (state:DUserState) =>{
  return({
    DUserList:state.list
})
}
const mapActionsToProps={
  fetchAllCandidates : _actions.fetchAll
}
export default connect(mapStateToProps,mapActionsToProps)(DUser)