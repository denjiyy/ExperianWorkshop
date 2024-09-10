import { ReactChild, ReactNode } from "react"
import * as S from "./elements"
import { BodyProps } from "../Typography"
import { HTMLHeadingPropsBody } from "../../types"
export interface DividerProps extends  HTMLHeadingPropsBody {
    text:"none" |string;
}
export const Divider = ({text}:DividerProps) => {

return(
    
    <S.DividerText>{text}</S.DividerText>
    
)

}