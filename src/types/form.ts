export interface SignUpProps {
    name:string;
    fullName:string;
    email:string;
    password:RegExp;
    confirmPassword:string;
    phoneNumber:string;
    address:string;
    dateOfBirth:string;
}
export interface LoginProps{
    email:string;
    password:string;
}