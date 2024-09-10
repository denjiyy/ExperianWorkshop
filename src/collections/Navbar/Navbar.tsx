import * as S from "./elements"

export const Navbar = () =>{

return(
    <S.NavBarDiv>
        <S.LogoDiv>
            {/* <S.Logo/> */}
            <S.LogoCaption>BankName</S.LogoCaption>
        </S.LogoDiv>
     <S.NavbarSect>
        <S.NavbarLink to="/">Loans</S.NavbarLink>
        <S.NavbarLink to="/">Payments</S.NavbarLink>
        <S.NavbarLink to="/">About us</S.NavbarLink>
        <S.NavbarLink to="/">We're hiring</S.NavbarLink>
     </S.NavbarSect>
    </S.NavBarDiv>
)

};