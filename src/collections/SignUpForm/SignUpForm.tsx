import * as S from "./elements";
import { useZodForm } from "../../hooks";
import { registerFormSchema } from "../../schemas";
import { useEffect, useState } from "react";

export const SignUpForm = ({ ...props }) => {
  const [hasError, setHasError] = useState<boolean>(false);
  const [nError, setError] = useState<string>("");
  const { formState:{errors,isSubmitting},register,control, handleSubmit } = useZodForm(registerFormSchema, {
    defaultValues: {
      username:"",
      email: "",
      password: "",
      confirmPassword:"",
      phoneNumber:"",
      dateOfBirth:"",
    },
  });
  useEffect(() => {
    if (Object.keys(errors).length > 0) {
      setHasError(true);
      console.log(hasError)

    } else {
      setHasError(false);
      console.log(hasError)
    }
  }, [errors]);
  const submitHandler = handleSubmit(async({username,email,password,confirmPassword,phoneNumber,dateOfBirth})=>{

    try {
      console.log(email, password); 
    } catch (error:any) {
      // Handle the submission error here if necessary
      setHasError(true);
      setError(error.message);
      console.log(hasError)
         }  })
         const ErrorHandler = ()=>{
          if (Object.keys(errors).length > 0) {
            setHasError(true);
            console.log(hasError)
      
          } else {
            setHasError(false);
            console.log(hasError)
          }
         }
      
  return (
    // <S.FormContainer {...props}>
    //   <S.FormInput
    //     control={control}
    //     name="email"
    //     type="email"
    //     placeholder="Enter Email"
    //     path = "M14.1666 8.33333V6.66667C14.1666 4.36548 12.3011 2.5 9.99992 2.5C7.69873 2.5 5.83325 4.36548 5.83325 6.66667V8.33333M9.99992 12.0833V13.75M7.33325 17.5H12.6666C14.0667 17.5 14.7668 17.5 15.3016 17.2275C15.772 16.9878 16.1544 16.6054 16.3941 16.135C16.6666 15.6002 16.6666 14.9001 16.6666 13.5V12.3333C16.6666 10.9332 16.6666 10.2331 16.3941 9.69836C16.1544 9.22795 15.772 8.8455 15.3016 8.60582C14.7668 8.33333 14.0667 8.33333 12.6666 8.33333H7.33325C5.93312 8.33333 5.23306 8.33333 4.69828 8.60582C4.22787 8.8455 3.84542 9.22795 3.60574 9.69836C3.33325 10.2331 3.33325 10.9332 3.33325 12.3333V13.5C3.33325 14.9001 3.33325 15.6002 3.60574 16.135C3.84542 16.6054 4.22787 16.9878 4.69828 17.2275C5.23306 17.5 5.93312 17.5 7.33325 17.5Z"
    //     stroke="#14151A"
    //     strokeOpacity={0.4}
    //   />
    //   <S.FormInput
    //     control={control}
    //     name="password"
    //     type="password"
    //     placeholder="Enter Password"
    //     path = "M14.1666 8.33333V6.66667C14.1666 4.36548 12.3011 2.5 9.99992 2.5C7.69873 2.5 5.83325 4.36548 5.83325 6.66667V8.33333M9.99992 12.0833V13.75M7.33325 17.5H12.6666C14.0667 17.5 14.7668 17.5 15.3016 17.2275C15.772 16.9878 16.1544 16.6054 16.3941 16.135C16.6666 15.6002 16.6666 14.9001 16.6666 13.5V12.3333C16.6666 10.9332 16.6666 10.2331 16.3941 9.69836C16.1544 9.22795 15.772 8.8455 15.3016 8.60582C14.7668 8.33333 14.0667 8.33333 12.6666 8.33333H7.33325C5.93312 8.33333 5.23306 8.33333 4.69828 8.60582C4.22787 8.8455 3.84542 9.22795 3.60574 9.69836C3.33325 10.2331 3.33325 10.9332 3.33325 12.3333V13.5C3.33325 14.9001 3.33325 15.6002 3.60574 16.135C3.84542 16.6054 4.22787 16.9878 4.69828 17.2275C5.23306 17.5 5.93312 17.5 7.33325 17.5Z"
    //     stroke="#14151A"
    //     strokeOpacity={0.4}
    //   />
    // </S.FormContainer>
    <S.PageContainer>
      <S.Container>
        <S.FormContainer >
        <S.H3Bd>Create your Account</S.H3Bd>
        <S.BodyLg>Let's get started with Bank Management</S.BodyLg>
        <S.FormInputContainer {...props} onSubmit={submitHandler}>
          <S.Buttonn long variant="primary">Sign Up with Bank</S.Buttonn>
<S.Divider text="none"></S.Divider>
          <S.FormDiv>
            <S.LabelInput>Email</S.LabelInput>
            <S.FormInput
        control={control}
        name="username"
        type="username"
        placeholder="Enter Name"
        path = "M14.1666 8.33333V6.66667C14.1666 4.36548 12.3011 2.5 9.99992 2.5C7.69873 2.5 5.83325 4.36548 5.83325 6.66667V8.33333M9.99992 12.0833V13.75M7.33325 17.5H12.6666C14.0667 17.5 14.7668 17.5 15.3016 17.2275C15.772 16.9878 16.1544 16.6054 16.3941 16.135C16.6666 15.6002 16.6666 14.9001 16.6666 13.5V12.3333C16.6666 10.9332 16.6666 10.2331 16.3941 9.69836C16.1544 9.22795 15.772 8.8455 15.3016 8.60582C14.7668 8.33333 14.0667 8.33333 12.6666 8.33333H7.33325C5.93312 8.33333 5.23306 8.33333 4.69828 8.60582C4.22787 8.8455 3.84542 9.22795 3.60574 9.69836C3.33325 10.2331 3.33325 10.9332 3.33325 12.3333V13.5C3.33325 14.9001 3.33325 15.6002 3.60574 16.135C3.84542 16.6054 4.22787 16.9878 4.69828 17.2275C5.23306 17.5 5.93312 17.5 7.33325 17.5Z"
        stroke="#14151A"
        strokeOpacity={0.4}
      />
      <S.ErrorMess>{errors.username?.message?.toString()}</S.ErrorMess>
          </S.FormDiv>
          <S.FormDiv>
            <S.LabelInput>Email</S.LabelInput>
            <S.FormInput
        control={control}
        name="email"
        type="email"
        placeholder="Enter Email"
        path = "M14.1666 8.33333V6.66667C14.1666 4.36548 12.3011 2.5 9.99992 2.5C7.69873 2.5 5.83325 4.36548 5.83325 6.66667V8.33333M9.99992 12.0833V13.75M7.33325 17.5H12.6666C14.0667 17.5 14.7668 17.5 15.3016 17.2275C15.772 16.9878 16.1544 16.6054 16.3941 16.135C16.6666 15.6002 16.6666 14.9001 16.6666 13.5V12.3333C16.6666 10.9332 16.6666 10.2331 16.3941 9.69836C16.1544 9.22795 15.772 8.8455 15.3016 8.60582C14.7668 8.33333 14.0667 8.33333 12.6666 8.33333H7.33325C5.93312 8.33333 5.23306 8.33333 4.69828 8.60582C4.22787 8.8455 3.84542 9.22795 3.60574 9.69836C3.33325 10.2331 3.33325 10.9332 3.33325 12.3333V13.5C3.33325 14.9001 3.33325 15.6002 3.60574 16.135C3.84542 16.6054 4.22787 16.9878 4.69828 17.2275C5.23306 17.5 5.93312 17.5 7.33325 17.5Z"
        stroke="#14151A"
        strokeOpacity={0.4}
      />
      <S.ErrorMess>{errors.email?.message?.toString()}</S.ErrorMess>
          </S.FormDiv>
          <S.FormDiv>
            <S.LabelInput>Email</S.LabelInput>
            <S.FormInput
        control={control}
        name="password"
        type="password"
        placeholder="Enter Password"
        path = "M14.1666 8.33333V6.66667C14.1666 4.36548 12.3011 2.5 9.99992 2.5C7.69873 2.5 5.83325 4.36548 5.83325 6.66667V8.33333M9.99992 12.0833V13.75M7.33325 17.5H12.6666C14.0667 17.5 14.7668 17.5 15.3016 17.2275C15.772 16.9878 16.1544 16.6054 16.3941 16.135C16.6666 15.6002 16.6666 14.9001 16.6666 13.5V12.3333C16.6666 10.9332 16.6666 10.2331 16.3941 9.69836C16.1544 9.22795 15.772 8.8455 15.3016 8.60582C14.7668 8.33333 14.0667 8.33333 12.6666 8.33333H7.33325C5.93312 8.33333 5.23306 8.33333 4.69828 8.60582C4.22787 8.8455 3.84542 9.22795 3.60574 9.69836C3.33325 10.2331 3.33325 10.9332 3.33325 12.3333V13.5C3.33325 14.9001 3.33325 15.6002 3.60574 16.135C3.84542 16.6054 4.22787 16.9878 4.69828 17.2275C5.23306 17.5 5.93312 17.5 7.33325 17.5Z"
        stroke="#14151A"
        strokeOpacity={0.4}
      />
      <S.ErrorMess>{errors.password?.message?.toString()}</S.ErrorMess>
          </S.FormDiv>
          <S.FormDiv>
            <S.LabelInput>Email</S.LabelInput>
            <S.FormInput
        control={control}
        name="confirmPassword"
        type="confirmPassword"
        placeholder="Enter Password Again"
        path = "M14.1666 8.33333V6.66667C14.1666 4.36548 12.3011 2.5 9.99992 2.5C7.69873 2.5 5.83325 4.36548 5.83325 6.66667V8.33333M9.99992 12.0833V13.75M7.33325 17.5H12.6666C14.0667 17.5 14.7668 17.5 15.3016 17.2275C15.772 16.9878 16.1544 16.6054 16.3941 16.135C16.6666 15.6002 16.6666 14.9001 16.6666 13.5V12.3333C16.6666 10.9332 16.6666 10.2331 16.3941 9.69836C16.1544 9.22795 15.772 8.8455 15.3016 8.60582C14.7668 8.33333 14.0667 8.33333 12.6666 8.33333H7.33325C5.93312 8.33333 5.23306 8.33333 4.69828 8.60582C4.22787 8.8455 3.84542 9.22795 3.60574 9.69836C3.33325 10.2331 3.33325 10.9332 3.33325 12.3333V13.5C3.33325 14.9001 3.33325 15.6002 3.60574 16.135C3.84542 16.6054 4.22787 16.9878 4.69828 17.2275C5.23306 17.5 5.93312 17.5 7.33325 17.5Z"
        stroke="#14151A"
        strokeOpacity={0.4}
      />
      <S.ErrorMess>{errors.confirmPassword?.message?.toString()}</S.ErrorMess>
          </S.FormDiv>
          <S.FormDiv>
            <S.LabelInput>Password</S.LabelInput>
            <S.FormInput
        control={control}
        name="phoneNumber"
        type="phoneNumber"
        placeholder="Enter Phone Number"
        path = "M14.1666 8.33333V6.66667C14.1666 4.36548 12.3011 2.5 9.99992 2.5C7.69873 2.5 5.83325 4.36548 5.83325 6.66667V8.33333M9.99992 12.0833V13.75M7.33325 17.5H12.6666C14.0667 17.5 14.7668 17.5 15.3016 17.2275C15.772 16.9878 16.1544 16.6054 16.3941 16.135C16.6666 15.6002 16.6666 14.9001 16.6666 13.5V12.3333C16.6666 10.9332 16.6666 10.2331 16.3941 9.69836C16.1544 9.22795 15.772 8.8455 15.3016 8.60582C14.7668 8.33333 14.0667 8.33333 12.6666 8.33333H7.33325C5.93312 8.33333 5.23306 8.33333 4.69828 8.60582C4.22787 8.8455 3.84542 9.22795 3.60574 9.69836C3.33325 10.2331 3.33325 10.9332 3.33325 12.3333V13.5C3.33325 14.9001 3.33325 15.6002 3.60574 16.135C3.84542 16.6054 4.22787 16.9878 4.69828 17.2275C5.23306 17.5 5.93312 17.5 7.33325 17.5Z"
        stroke="#14151A"
        strokeOpacity={0.4}
      />
      <S.ErrorMess>{errors.phoneNumber?.message?.toString()}</S.ErrorMess>
          </S.FormDiv>
          <S.FormDiv>
            <S.LabelInput>Email</S.LabelInput>
            <S.FormInput
        control={control}
        name="dateOfBirth"
        type="dateOfBirth"
        placeholder="Enter your date of Birth"
        path = "M14.1666 8.33333V6.66667C14.1666 4.36548 12.3011 2.5 9.99992 2.5C7.69873 2.5 5.83325 4.36548 5.83325 6.66667V8.33333M9.99992 12.0833V13.75M7.33325 17.5H12.6666C14.0667 17.5 14.7668 17.5 15.3016 17.2275C15.772 16.9878 16.1544 16.6054 16.3941 16.135C16.6666 15.6002 16.6666 14.9001 16.6666 13.5V12.3333C16.6666 10.9332 16.6666 10.2331 16.3941 9.69836C16.1544 9.22795 15.772 8.8455 15.3016 8.60582C14.7668 8.33333 14.0667 8.33333 12.6666 8.33333H7.33325C5.93312 8.33333 5.23306 8.33333 4.69828 8.60582C4.22787 8.8455 3.84542 9.22795 3.60574 9.69836C3.33325 10.2331 3.33325 10.9332 3.33325 12.3333V13.5C3.33325 14.9001 3.33325 15.6002 3.60574 16.135C3.84542 16.6054 4.22787 16.9878 4.69828 17.2275C5.23306 17.5 5.93312 17.5 7.33325 17.5Z"
        stroke="#14151A"
        strokeOpacity={0.4}
      />
      <S.ErrorMess>{errors.dateOfBirth?.message?.toString()}</S.ErrorMess>
          </S.FormDiv>
        <S.LabelInput>Don't Have an Account? <S.NavbarLink to="/"> Sign up</S.NavbarLink></S.LabelInput>
        {hasError ? <S.Buttonn variant="secondary" type="submit" onClick={ErrorHandler} disabled long >Sign up</S.Buttonn>:
        <S.Buttonn variant="secondary" type="submit" onClick={ErrorHandler}  long >Sign up</S.Buttonn>
        }
          
        </S.FormInputContainer>
        </S.FormContainer>
        <S.Image src=""/>
      </S.Container>
    </S.PageContainer>
  );
};
