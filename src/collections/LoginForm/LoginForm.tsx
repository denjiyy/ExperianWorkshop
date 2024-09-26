import * as S from "./elements";
import { useZodForm } from "../../hooks";
import { loginFormSchema } from "../../schemas";
import { useEffect, useState,useRef, useContext } from "react";
import { DUserState } from "../../types";
import * as _actions from "../../actions/Duser"
import { connect, useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
// import { useLoginMutation } from "../../actions/authApiSlice";
// import { setCredentials } from "../../actions/authSlice";
import { LoginProps } from "../../types/form";
import { RootState } from "@/src/actions/store";


//import AuthContext from "@/src/axios_auth/context/AuthProvider";

//import axios from "../../axios_auth/api/axios";
// const LOGIN_URL = 'api/auth'

interface responseProps{
  token?:string;
  administrator:boolean;
}
export const LoginFormToConnect = (props:any) => {
  //const {setAuth} = useContext(AuthContext)
const emailRef = useRef()
const errRef = useRef()
const [user,setUser] = useState('')
const [pwd,setPwd] = useState('')
const [errMsg,setErrMsg] = useState('')
const navigate = useNavigate()

// const [login, {isLoading}] = useLoginMutation()
const dispatch = useDispatch()

useEffect(()=>{
    // emailRef.current.focus()
},[])
useEffect(()=>{
  setErrMsg('')
},[user,pwd])

  const { formState:{errors,isSubmitting},register,control, handleSubmit } = useZodForm(loginFormSchema, {
    defaultValues: {
      username: "",
      password: "",
    },
  });
  const submitHandler = handleSubmit(async (data: LoginProps) => {
    if (Object.keys(errors).length === 0) {
      try {
        // Await the login API call
        const response : responseProps = await props.loginuser(data);
        const {token,administrator} = props.dUserList[0];

        if(token){
          localStorage.setItem('token',token)
        localStorage.setItem('administrator',administrator)
        }
        
      }
        // Check if response exists and contains the expected data
     catch (error) {
        // Handle any errors during API call or processing
        console.error("An error occurred during login:", error);
      }
    }
     else {
      console.error("There are validation errors:", errors);
    }
});
  
  
  
  // };
      
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
// isLoading ? <h1>Loading...</h1> :(
    <S.PageContainer>
      <S.Container>
        <S.FormContainer >
        <S.H3Bd>Create your Account</S.H3Bd>
        <S.BodyLg>Let's get started with Bank Management</S.BodyLg>
        <S.FormInputContainer onSubmit={submitHandler}>
          <S.Buttonn long variant="primary">Sign Up with Bank</S.Buttonn>
<S.Divider text="none"></S.Divider>
          <S.FormDiv>
            <S.LabelInput>Email</S.LabelInput>
            <S.FormInput
        control={control}
        name="username"
        type="username"
        placeholder="Enter UserName"
        // ref={emailRef}
        //have to forward it as ref like the button component
        path = "M14.1666 8.33333V6.66667C14.1666 4.36548 12.3011 2.5 9.99992 2.5C7.69873 2.5 5.83325 4.36548 5.83325 6.66667V8.33333M9.99992 12.0833V13.75M7.33325 17.5H12.6666C14.0667 17.5 14.7668 17.5 15.3016 17.2275C15.772 16.9878 16.1544 16.6054 16.3941 16.135C16.6666 15.6002 16.6666 14.9001 16.6666 13.5V12.3333C16.6666 10.9332 16.6666 10.2331 16.3941 9.69836C16.1544 9.22795 15.772 8.8455 15.3016 8.60582C14.7668 8.33333 14.0667 8.33333 12.6666 8.33333H7.33325C5.93312 8.33333 5.23306 8.33333 4.69828 8.60582C4.22787 8.8455 3.84542 9.22795 3.60574 9.69836C3.33325 10.2331 3.33325 10.9332 3.33325 12.3333V13.5C3.33325 14.9001 3.33325 15.6002 3.60574 16.135C3.84542 16.6054 4.22787 16.9878 4.69828 17.2275C5.23306 17.5 5.93312 17.5 7.33325 17.5Z"
        stroke="#14151A"
        strokeOpacity={0.4}
      />
      <S.ErrorMess>{errors.username?.message?.toString()}</S.ErrorMess>
          </S.FormDiv>
          <S.FormDiv>
            <S.LabelInput>Password</S.LabelInput>
            <S.FormInput
        control={control}
        name="password"
        type="password"
        placeholder="Enter password"
        path = "M14.1666 8.33333V6.66667C14.1666 4.36548 12.3011 2.5 9.99992 2.5C7.69873 2.5 5.83325 4.36548 5.83325 6.66667V8.33333M9.99992 12.0833V13.75M7.33325 17.5H12.6666C14.0667 17.5 14.7668 17.5 15.3016 17.2275C15.772 16.9878 16.1544 16.6054 16.3941 16.135C16.6666 15.6002 16.6666 14.9001 16.6666 13.5V12.3333C16.6666 10.9332 16.6666 10.2331 16.3941 9.69836C16.1544 9.22795 15.772 8.8455 15.3016 8.60582C14.7668 8.33333 14.0667 8.33333 12.6666 8.33333H7.33325C5.93312 8.33333 5.23306 8.33333 4.69828 8.60582C4.22787 8.8455 3.84542 9.22795 3.60574 9.69836C3.33325 10.2331 3.33325 10.9332 3.33325 12.3333V13.5C3.33325 14.9001 3.33325 15.6002 3.60574 16.135C3.84542 16.6054 4.22787 16.9878 4.69828 17.2275C5.23306 17.5 5.93312 17.5 7.33325 17.5Z"
        stroke="#14151A"
        strokeOpacity={0.4}
      />
      <S.ErrorMess>{errors.password?.message?.toString()}</S.ErrorMess>
          </S.FormDiv>
          {/* <S.ErrorMess ref={errRef}>{errMsg ? "errmsg" : "offscreen"}</S.ErrorMess> */}
          {/* have to forward it as ref like the button element */}
        <S.LabelInput>Don't Have an Account? <S.NavbarLink to="/signup"> Sign up</S.NavbarLink></S.LabelInput>
        <S.Buttonn variant="secondary" type="submit"   long >Sign up</S.Buttonn>
        
          
        </S.FormInputContainer>
        </S.FormContainer>
        <S.Image src=""/>
      </S.Container>
    </S.PageContainer>
  );
};
const mapStateToProps = (state:RootState) =>{

  return({
    dUserList:state.DUser.list
})
}
const mapActionsToProps={
  createduser : _actions.create,
  loginuser: _actions.loginUser,
  fetchUser : _actions.fetchById,
  // updateduSer:_actions.update,
}
export const LoginForm = connect(mapStateToProps,mapActionsToProps)(LoginFormToConnect)
