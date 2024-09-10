import {styled,css} from "styled-components"
import { HTMLImageProps } from "../../types/index"
// const imgStyles =(({src,...props}:HTMLImageProps)=> css`
// width: 48px;
// height: 48px;
// `)
export const AvatarDiv = styled.div`
  width: 48px;
  height: 48px;
  display: flex;
  justify-content: center;
  align-items: center;
  border-radius: 50%;
  border: 3px solid rgba(20, 21, 26, 1);
  position: relative;
  margin-left:-10px;
  overflow: hidden; 
`
export const AvatarImage = styled.img<HTMLImageProps>(()=>css`
object-fit:cover;
`)


