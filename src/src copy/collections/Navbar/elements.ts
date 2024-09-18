import styled,{css} from "styled-components";
import { H4Md as _h4,BodyMdMd as _bodymdmd,Icon as _icon } from "../../components";
import { NavLink } from "react-router-dom";

export const NavBarDiv = styled.div(()=>css`
display:flex;
justify-content:space-between;
flex-direction:row;
margin-left:34px;
margin-right:30px;
margin-top:24px;
`)

export const Logo = styled(_icon)(()=>css`
width: 24.257px;
height: 37.041px;
flex-shrink: 0;
`)
export const ProfileLogo = (styled)(_icon)(()=>css`
width: 25px;
height: 25px;
flex-shrink: 0;
`)
export const LogoCaption = styled(_h4)(()=>css`
color:#14151A;
font-weight:700;
`)

export const LogoDiv = styled.div(()=>css`
    display:flex;

`)

export const NavbarSect = styled.div(()=>css`
display: inline-flex;
padding: 12px 22px 12px 24px;
justify-content: center;
align-items: flex-start;
gap: 23px;
border-radius: 18px;
border: 1px solid rgba(0, 0, 0, 0.20);
background: rgba(65, 0, 130, 0.05);
`)
const H4 = styled(_bodymdmd)(()=>css`
color:#14151A;
`)
export const NavbarLink = styled(NavLink)(()=>css`
text-decoration:none;
${H4}{
color:inherit;
}
`)
//should be modified to a link