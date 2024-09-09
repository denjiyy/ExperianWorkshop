import React, { forwardRef,ForwardedRef, ReactElement, Children } from 'react'
import * as S from "./elements" 
import type { HTMLButtonProps } from '../../types'
import {Icon} from "../Icon"
import type { HTMLHeadingPropsBody } from '../../types'
export interface ButtonProps extends HTMLButtonProps {
  variant ?: "primary" | "secondary" | "tertiary" | "ghost",
  iconPath ?:"none" | string,
  fill ?: "none"  | string,
  stroke?: "none" | string;
  strokeOpacity?: number;
  viewBox ?: string;
}

// export const Button = (isGame : boolean,children : ReactElement) => {
    
//   return (
//     <S.Button>{children}</S.Button>
//   )
// }
//BodyMDMD


export const Button = forwardRef(
  (
    { fill,variant,iconPath,children,stroke,viewBox,strokeOpacity,...props }: ButtonProps,
    ref: ForwardedRef<HTMLButtonElement>,
    

  ) => {
    return <S.Button {...props} variant={variant} ref={ref}>{children}{iconPath!=null && <Icon path={iconPath} viewBox={viewBox} stroke={stroke} strokeOpacity={strokeOpacity} fill={fill} xmlns="http://www.w3.org/2000/svg" />}
    </S.Button>;
  }
); //using ref to access the button


// export const Button = ({variant,target,icon,...props}:ButtonProps)=>{
//   return <S.Button {...props} variant={variant} icon={icon} target={target} />;
// }

/*
// Вашият компонент
import * as S from "./elements";
import { forwardRef, ForwardedRef } from "react";
import type { HTMLButtonProps } from "../../types";

export interface ButtonProps extends HTMLButtonProps {
  variant?: "primary" | "secondary";
}

export const Button = forwardRef(
  (
    { variant, ...props }: ButtonProps,
    ref: ForwardedRef<HTMLButtonElement>
  ) => {
    return <S.Button {...props} variant={variant} ref={ref} />;
  }
);

Button.displayName = "Button"; // Добавяне на displayName

export default Button;

*/

