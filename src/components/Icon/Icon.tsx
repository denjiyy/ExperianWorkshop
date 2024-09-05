import { HTMLSVGProps } from "../../types"
import * as S from "./elements"

export interface IconProps extends HTMLSVGProps{
    fill ?: "none" | string,
}

export const Icon = ({path,xmlns,viewBox,fill,...props}:IconProps)=>{
    return ( <S.Icon tabIndex={0} {...props} xmlns={xmlns} fill="none" viewBox="0 0 20 20">
          (
                <path 
                    d={path}
                    stroke={fill}
                    strokeOpacity="0.6"
                    strokeWidth="2"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                />
            )
        </S.Icon>
)}
//update the svg viewbox and fill
// and path strokeOpacity
//use ref to update the focus of the icon
//may used icon without path or xmlns and then render it at the certain button