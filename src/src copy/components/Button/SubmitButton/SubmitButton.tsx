import { ForwardedRef, forwardRef } from "react";
import { HTMLInputProps } from "../../../types";
import * as S from "./elements"
import { Icon } from "../../Icon";
  export interface SubmitButtonProps extends HTMLInputProps{

    variant ?: "primary" | "secondary" | "tertiary" | "ghost",
    iconPath ?:"none" | string,
    fill ?: "none"  | string,
    stroke?: "none" | string;
    strokeOpacity?: number;
    viewBox ?: string;
  } 

  export const SubmitButton = forwardRef(
    (
      { fill,variant,iconPath,children,stroke,viewBox,value,strokeOpacity,...props }: SubmitButtonProps,
      ref: ForwardedRef<HTMLInputElement>,
      
  
    ) => {
      return <S.SubmitButton type="submit" value={value} {...props} variant={variant} ref={ref}/>

    }
  );