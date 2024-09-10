import styled,{css} from "styled-components";
import { SubmitButtonProps } from "./SubmitButton";
import { theme } from "../../../styles";
const buttonStyles = {
    primary:css`
background: #FFF;
color:#14151A;
box-shadow: 0px 1px 2px 0px rgba(20, 21, 26, 0.05);
&:hover{
background: #B8B8B8;
}
&:focused{

}
`
,
    secondary:css`
background: #14151A;
color:#fff;
box-shadow: 0px 1px 2px 0px rgba(20, 21, 26, 0.05);
&:hover{
background:rgba(211,195,255,0.8);
}

`,
    tertiary: css`
background: rgba(255, 255, 255, 0.16);
color:#fff;


`,
    ghost:``

}
export const SubmitButton = styled.input<SubmitButtonProps>(({variant})=>css`

display: inline-flex;
padding: 10px 12px;
justify-content: center;
align-items: center;
gap: 4px;
border-radius:12px;
border:none;
transition:0.5s ease-in;
&:hover{
transition:0.5s ease-in;}
${theme.typography.body.medium.medium}
}
    ${variant == "primary" && buttonStyles.primary}
    ${variant == "secondary" && buttonStyles.secondary}
    ${variant =="tertiary" && buttonStyles.tertiary}
    ${variant=="ghost" && buttonStyles.ghost}
  

`)