import { styled, css } from "styled-components";
import { FormInput as _form } from "../../components";

export const FormInput = styled(_form)(() => css``);
export const FormContainer = styled.form(
  ({ theme: { colors, breakpoint } }) => css`
    display: flex;
    flex-direction: column;
    padding-top: 14px;
  `
);
