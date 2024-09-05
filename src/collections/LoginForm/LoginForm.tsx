import * as S from "./elements";
import { useZodForm } from "../../hooks";
import { loginFormSchema } from "../../schemas";

export const LoginForm = ({ ...props }) => {
  const { control, handleSubmit } = useZodForm(loginFormSchema, {
    defaultValues: {
      email: "",
      password: "",
    },
  });
  return (
    <S.FormContainer {...props}>
      <S.FormInput
        control={control}
        name="email"
        type="email"
        placeholder="Enter Email"
      />
      <S.FormInput
        control={control}
        name="password"
        type="password"
        placeholder="Enter Password"
      />
    </S.FormContainer>
  );
};
