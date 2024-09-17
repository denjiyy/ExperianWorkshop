import styled, { css } from "styled-components";
import { Icon as _icon,H3Bd as _h3,H4Md as _h4,BodyLg as _bodylg,BodyMdMd as _bodymdmd,Image as _image,BodyMdSb as _bodymdsb } from "../../components";

// export const SectionContainer = styled.div(()=>css`
// display:flex;
// flex-direction:row;
// justify-content:center;
// `)
// export const ListSettingContainer = styled.div(()=>css`
// width:95%;
// display:flex;
// flex-direction:column;
// align-items:flex-start;
// `)
// export const HeadingSettings = styled(_h3)(()=>css`

// `)
// export const AccountDiv = styled.div(()=>css`
//     display:grid;
//     grid-template-columns:3fr 9fr;
// `)

// ----> for the page

export const AccountContainer = styled.div(()=>css`
    margin: 30px 16px 20px 16px;
    display:flex;
    flex-direction:column;
    align-items:flex-start;
    gap:20px;
`)
export const Header = styled(_h4)(()=>css`
`)
const AccountDivPattern = styled.div(()=>css`
border-radius: var(--radius-xl, 12px);
border: 1px solid var(--Input-field-Light-mode-Stroke-Primary, #DEE0E3);
background: var(--Input-field-Light-mode-Fill-Primary, #FFF);
box-shadow: 0px 1px 2px 0px rgba(20, 21, 26, 0.05);
width:100%;
padding:10px 12px;

`)
export const AccountNameDiv = styled(AccountDivPattern)(()=>css`
display:flex;
flex-direction:row;
gap:10px;
`)
export const ProfilePicture = styled(_image)(()=>css`
border-radius: 200px;
width:90px;
height:90px;
`)
export const PrfofileInfo = styled.div(()=>css`
display:flex;
flex-direction:column;
    justify-content:center;

`)
export const AccountName = styled(_bodylg)`
font-size:16px;
`
export const AccountLocation = styled(_bodymdmd)(()=>css`
opacity:0.4;
`)

export const PersonalInformationaDiv = styled(AccountDivPattern)`
    display:flex;
    flex-direction:column;
`
export const PersonalHeading = styled(_bodymdmd)(()=>css`
    font-weight:800;
`)
export const PersonalTable = styled.div(()=>css`
margin-top:20px;
display:grid;
    grid-template-columns:1fr 1fr;
    grid-template-rows:repeat(3,1fr);
    grid-row-gap:10px;
    gap:15px;
`)
export const InfoDiv = styled.div(()=>css`
    display:flex;
    flex-direction:column;
`)
export const InfoHeader = styled(_bodymdsb)(()=>css`
opacity:0.5;
`)
export const InfoContent = styled(_bodymdsb)(()=>css`
`)
export const CreditCardInfo = styled(AccountDivPattern)`
display:flex;
flex-direction:column;
`
export const CreditCardTable = styled.div(()=>css`
margin-top:20px;
display:grid;
grid-template-columns:1fr 1fr;
    grid-row-gap:10px;
`)



