import { styled, css } from "styled-components";
import {FormInput as _form,Divider as _divider, H3Bd as _h3bd,BodyLg as _bodylg,Button as _button,BodyMdRg as _bodymdRg,Image as _image,SubmitButton as _submit, ButtonProps } from "../../components";
import { NavLink } from "react-router-dom";

interface SignUpButtonProps extends ButtonProps{
  long?:boolean;
  disabled?:boolean
}
export const FormInput = styled(_form)(() => css`
width:100%;
`);
export const FormInputContainer = styled.form(
  () => css`
    display: flex;
    flex-direction: column;
    margin-bottom:20px;
    
  `
);
export const PageContainer = styled.div(()=>css`
  width:100%;
  display:flex;
  justify-content:center;
`)
export const Container = styled.div(()=>css`
  width:90%;
  margin-top:20px;
  display:grid;
  grid-template-columns:1fr 2fr;
  gap:20px; 
  & ${H3Bd}:first-child {
    margin-bottom:10px;
  }
    & ${Buttonn}:first-child{
    margin-bottom:20px;
}
`)
export const FormContainer = styled.div(()=>css`
  display:flex;
  flex-direction:column;
  justify-content:center;
    box-shadow: 0px 1px 2px 0px rgba(20, 21, 26, 0.2);
    padding: 10px 12px;
    border-radius:12px;

`)
export const FormDiv = styled.div(()=>css`
  display: flex;
flex-direction: column;
align-items: flex-start;
gap: var(--spacing-md, 8px);
align-self: stretch;
`)
export const H3Bd = styled(_h3bd)(()=>css`
  
`)
export const BodyLg = styled(_bodylg)(()=>css`
opacity:0.5;
margin-bottom:30px;
`)
export const LabelInput = styled(_bodylg)(()=>css`

`)
export const BodyMdRg = styled(_bodymdRg)(()=>css`

`)
// export const Heading4 = styled(({ variant , ...props }: H4Props) => (
//   <h4 {...props} />
// ))`
//   ${({ variant }) => variant && theme.typography.h4[variant]}
// `;
export const Button = styled(_button)(()=>css`
`)
export const Buttonn = styled(({long,...props}:SignUpButtonProps)=>(
  <Button {...props}/>
))`
padding:${({long})=>long? "10px" : "default-padding"};
margin-top:15px;
  background-color: ${({ disabled }) => (disabled ? "#d3d3d3" : "#007bff")}; /* Example colors */
  cursor: ${({ disabled }) => (disabled ? "not-allowed" : "pointer")};
  opacity: ${({ disabled }) => (disabled ? 0.6 : 1)};
`
export const Image = styled(_image)(()=>css`

`)
export const NavbarLink = styled(NavLink)(()=>css`
text-decoration:none;
color:#D0BCFF;

`)
export const Divider = styled(_divider)(()=>css`
  color:#14151A;
`)
export const ErrorMess = styled(_bodymdRg)(()=>css`
  color:#ff0033;
`)

