import { HTMLSVGProps } from "../../types"
import * as S from "./elements"

export interface IconProps extends HTMLSVGProps {
    path: string; 
    fill?: "none" | string;
    stroke?: "none" | string;
    strokeOpacity?: number;
    viewBox ?: string;
  }
  
  // Icon component using the refined IconProps interface
  export const Icon = ({
    path,
    xmlns = "http://www.w3.org/2000/svg", // Default value for xmlns
    viewBox = "0 0 24 24", // Default value for viewBox, or adjust as needed
    fill = "none",
    stroke,
    strokeOpacity,
  }: IconProps) => {
    return (
      <S.Icon
        xmlns={xmlns}
        fill={fill}
        viewBox={viewBox}
      >
        <path
          d={path}
          stroke={stroke}
          strokeOpacity={strokeOpacity}
          strokeWidth="2"
          strokeLinecap="round"
          strokeLinejoin="round"
        />
      </S.Icon>
    );
  };
//update the svg viewbox and fill
// and path strokeOpacity
//use ref to update the focus of the icon
//may used icon without path or xmlns and then render it at the certain button