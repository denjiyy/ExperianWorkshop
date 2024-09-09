import styled, { css } from "styled-components";
import { FormInputProps } from "./FormInput";
import { Icon as _icon } from "../Icon";

// export const FormInput = styled.input<{ variant?: string }>(
//   ({ variant }) => css`
//     background-color: #242424;
//     color: #d9d9d9;
//     border: 2px solid #4ee1ad;
//     border-radius: 5px;
//     padding: 8px;
//     outline: none;
//     transition: border-color 0.3s;
//     font-size: 20px;

//     &:hover,
//     &:focus {
//       border-color: #d9d9d9;

//       ${variant === "alert" &&
//       css`
//         border: 2px solid red;
//       `}
//     }

//     ${variant === "alert" &&
//     css`
//       border: 2px solid red;
//     `}
//   `
// );
export const Icon = styled(_icon)<{variant?:string}>(({variant})=>css`
  
`)
export const FormInputContainer = styled.div(()=>css`
display: flex;
padding: 10px 14px;
align-items: center;
gap: var(--spacing-xs, 4px);
align-self: stretch;
border-radius: var(--radius-xl, 12px);
border: 1px solid var(--Input-field-Light-mode-Stroke-Primary, #DEE0E3);
background: var(--Input-field-Light-mode-Fill-Primary, #FFF);
box-shadow: 0px 1px 2px 0px rgba(20, 21, 26, 0.05);
`)
export const FormInput = styled.input<{variant?:string}>(({variant})=>css`
display: flex;
padding: 10px 14px;
align-items: center;
gap: var(--spacing-xs, 4px);
align-self: stretch;
border-radius: var(--radius-xl, 12px);
border: 1px solid var(--Input-field-Light-mode-Stroke-Primary, #DEE0E3);
background: var(--Input-field-Light-mode-Fill-Primary, #FFF);
box-shadow: 0px 1px 2px 0px rgba(20, 21, 26, 0.05);
 &:hover,
    &:focus {
      border:1px solid rgba(0,0,0,0.5);

      ${variant === "alert" &&
      css`
        border: 1px solid #31111D;
      `}
    }

    ${variant === "alert" &&
    css`
      border: 1px solid #31111D;
    `}

`)

