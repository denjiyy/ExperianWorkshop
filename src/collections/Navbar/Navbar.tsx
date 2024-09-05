import * as S from "./elements"

export const Navbar = () =>{

return(
    <S.NavBarDiv>
        <S.LogoDiv>
            <S.Logo/>
            <S.LogoCaption>LiftMe</S.LogoCaption>
        </S.LogoDiv>
     <S.NavbarSect>
        <S.NavbarLink to="/">Home</S.NavbarLink>
        <S.NavbarLink to="/">Surveys</S.NavbarLink>
        <S.NavbarLink to="/">Rewards</S.NavbarLink>
        <S.NavbarLink to="/">About us</S.NavbarLink>
        <S.NavbarLink to="/">Blog</S.NavbarLink>
        <S.NavbarLink to="/">We're hiring</S.NavbarLink>
     </S.NavbarSect>
    </S.NavBarDiv>
)

};