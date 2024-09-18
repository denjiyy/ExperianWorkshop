import { useSelector, UseSelector } from "react-redux";
import { selectCurrentUser,selectCurrentToken } from "../../../actions/authSlice";
import { Link } from "react-router-dom";
import React from 'react'

export const Welcome = () => {
    const user = useSelector(selectCurrentUser)
    const token = useSelector(selectCurrentToken)
    const welcome = user? `Welcome ${user}!` : `Welcome!`
    const tokenAbbr = `${token.slice(0,9)}...`
  return (
    <>
    <div>{welcome}</div>
    <p>Token:{tokenAbbr}</p>
    </>
  )
}

