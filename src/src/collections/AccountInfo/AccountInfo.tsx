import * as S from "./elements"
import AvatarJane from "../../images/Avatars/Avatar1.png"

export const AccountInfo = () =>{

    return(
        <S.AccountContainer>
            <S.Header>My profile</S.Header>
            <S.AccountNameDiv>
                <S.ProfilePicture src={AvatarJane}/>
                <S.PrfofileInfo>
                    <S.AccountName>Yavor Ganev</S.AccountName>
                    <S.AccountLocation> Sofia,Bulgaria</S.AccountLocation>
                </S.PrfofileInfo>
                </S.AccountNameDiv>
                <S.PersonalInformationaDiv>
                    <S.PersonalHeading>Personal Information</S.PersonalHeading>
                    <S.PersonalTable>
                    <S.InfoDiv>
                        <S.InfoHeader>First Name</S.InfoHeader>
                        <S.InfoContent>Yavor</S.InfoContent>
                    </S.InfoDiv>
                    <S.InfoDiv>
                        <S.InfoHeader>Last Name</S.InfoHeader>
                        <S.InfoContent>Ganev</S.InfoContent>
                    </S.InfoDiv>
                    <S.InfoDiv>
                        <S.InfoHeader>Email Address</S.InfoHeader>
                        <S.InfoContent>yavor@gmail.com</S.InfoContent>
                    </S.InfoDiv>
                    <S.InfoDiv>
                        <S.InfoHeader>Phone</S.InfoHeader>
                        <S.InfoContent>+359 88 8888 000</S.InfoContent>
                    </S.InfoDiv>
                    <S.InfoDiv>
                        <S.InfoHeader>Occupation</S.InfoHeader>
                        <S.InfoContent>FrontEnd Dev</S.InfoContent>
                    </S.InfoDiv>
                    </S.PersonalTable>
                </S.PersonalInformationaDiv>
                <S.CreditCardInfo>
                    <S.PersonalHeading>Credit Card</S.PersonalHeading>
                    <S.CreditCardTable>
                    <S.InfoDiv>
                        <S.InfoHeader>Card Number</S.InfoHeader>
                        <S.InfoContent>**** **** **** 3457</S.InfoContent>
                    </S.InfoDiv>
                    <S.InfoDiv>
                        <S.InfoHeader>Expiry date</S.InfoHeader>
                        <S.InfoContent>09/25</S.InfoContent>
                    </S.InfoDiv>
                    <S.InfoDiv>
                        <S.InfoHeader>CVV</S.InfoHeader>
                        <S.InfoContent>215</S.InfoContent>
                    </S.InfoDiv>
                    </S.CreditCardTable>
                </S.CreditCardInfo>
            
        </S.AccountContainer>
    )
}