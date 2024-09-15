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
        <S.NavbarLink to="/"><S.ProfileLogo viewBox="0 0 28 28" path="M5.64537 23.298C6.40578 21.5065 8.18115 20.25 10.25 20.25L17.75 20.25C19.8188 20.25 21.5942 21.5065 22.3546 23.298M19 10.875C19 13.6364 16.7614 15.875 14 15.875C11.2386 15.875 9 13.6364 9 10.875C9 8.11358 11.2386 5.875 14 5.875C16.7614 5.875 19 8.11358 19 10.875ZM26.5 14C26.5 20.9036 20.9036 26.5 14 26.5C7.09644 26.5 1.5 20.9036 1.5 14C1.5 7.09644 7.09644 1.5 14 1.5C20.9036 1.5 26.5 7.09644 26.5 14Z" fill="none" stroke="#14151A"/></S.NavbarLink>
     </S.NavbarSect>
    </S.NavBarDiv>
)

};