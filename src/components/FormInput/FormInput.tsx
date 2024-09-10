import React from "react";
import * as S from "./elements";
import { Control, FieldValues, Path, useController } from "react-hook-form";
import { HTMLInputProps } from "../../types";
import { IconProps } from "../Icon";

export interface FormInputProps<T extends FieldValues = any>
  extends Omit<HTMLInputProps, "name" | "defaultValue"> {
  name: Path<T>;
  label?: string;
//   control: Control<T, any>;
control:any;
  variant?: string;
  textarea?: boolean;
  rows?: number; // Add rows for textarea
}
type CombinedFormInputProps<T extends FieldValues = any> = FormInputProps<T> & IconProps;


export const FormInput = <T extends FieldValues = any>({
  placeholder,
  variant,
  name,
  control,
  textarea,
  rows,
  path,
  fill,
  viewBox = "0 0 20 20", // Default value for viewBox
  stroke,
  strokeOpacity,
  ...props
}: CombinedFormInputProps<T>) => {
  const {
    field: { onChange, onBlur, value, ref },
    fieldState: { invalid, isTouched, isDirty, error },
  } = useController({
    name,
    control,
    rules: { required: true },
    defaultValue: "" as any,
  });

  const InputComponent = textarea ? "textarea" : "input";

  return(
    <S.FormInputContainer>
      <S.Icon  path={path} xmlns="http://www.w3.org/2000/svg" viewBox="0 0 20 20" fill="none" stroke={stroke} strokeOpacity={strokeOpacity}  />
      <S.FormInput
      {...props}
              as={InputComponent}
              variant="alert"
              placeholder={placeholder}
              onChange={onChange}
              onBlur={onBlur}
              value={value}
              ref={ref}
              rows={textarea ? rows : undefined}
      />

    </S.FormInputContainer>
  )
  // if (error) {
  //   return (
  //     <>
  //       <S.FormInput
  //         {...props}
  //         as={InputComponent}
  //         variant="alert"
  //         placeholder={placeholder}
  //         onChange={onChange}
  //         onBlur={onBlur}
  //         value={value}
  //         ref={ref}
  //         rows={textarea ? rows : undefined} // Only include rows for textarea
  //       />
  //     </>
  //   );
  // }
  // return (
  //   <>
  //     <S.FormInput
  //       {...props}
  //       as={InputComponent}
  //       placeholder={placeholder}
  //       onChange={onChange}
  //       onBlur={onBlur}
  //       value={value}
  //       ref={ref}
  //       rows={textarea ? rows : undefined} // Only include rows for textarea
  //     />
  //   </>
  //);
};
