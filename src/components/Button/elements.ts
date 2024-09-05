import styled,{css} from "styled-components";
import type { ButtonProps } from "./Button";
import { HTMLButtonProps } from "../../types";
import { theme } from "../../styles";
const buttonStyles = {
    primary:css`
    display: inline-flex;
padding: 10px 12px;
justify-content: center;
align-items: center;
gap: 4px;
border-radius:12px;
background: #FFF;
border:none;
color:#14151A;
box-shadow: 0px 1px 2px 0px rgba(20, 21, 26, 0.05);
&:hover{
background: #B8B8B8;
}
&:focused{

}
& > * {
${theme.typography.body.medium.medium}
}

`
,
    secondary:css`
    display: inline-flex;
padding: 10px 12px;
justify-content: center;
align-items: center;
gap: 4px;
border-radius:12px;
border:none;
background: #14151A;
color:#fff;
box-shadow: 0px 1px 2px 0px rgba(20, 21, 26, 0.05);
&:hover{
background:#0A0B10;
}
& > * {
${theme.typography.body.medium.medium}
}
`,
    tertiary: css`
    display: inline-flex;
    padding:10px 12px;
justify-content: center;
align-items: center;
border:none;
gap: var(--spacing-xs, 4px);
border-radius: var(--radius-xl, 12px);
background: rgba(255, 255, 255, 0.08);
color:#fff;
& > * {
${theme.typography.body.medium.medium}
}

`,
    ghost:``

}
export const Button = styled.button<ButtonProps>(
    ({ variant }) => css`
    ${variant == "primary" && buttonStyles.primary}
    ${variant == "secondary" && buttonStyles.secondary}
    ${variant =="tertiary" && buttonStyles.tertiary}
    ${variant=="ghost" && buttonStyles.ghost}
  `
);

/*
import styled, { css } from "styled-components";
import type { ButtonProps } from "./Button";

const buttonStyles = {
  primary: css`
    height: 40px;
    font-size: 1.5rem;
    outline: transparent;
    background-color: transparent;
    color: #d9d9d9;
    border: transparent;
    border-bottom: 3px solid #4ee1ad;
    cursor: pointer;
    transition: 200ms ease-in-out;
    margin-top: 20px;

    width: 100%;
    max-width: 200px;

    &:hover {
      color: #4ee1ad;
    }
  `,
  secondary: css`
    height: 40px;
    font-size: 1.5rem;
    outline: transparent;
    background-color: transparent;
    color: #d9d9d9;
    border: transparent;
    border: 3px solid #4ee1ad;
    cursor: pointer;
    transition: 200ms ease-in-out;
    margin-top: 20px;
    border-radius: 10px;

    width: 100%;
    max-width: 200px;

    &:hover {
      color: #4ee1ad;
    }
  `,
};

export const Button = styled.button<ButtonProps>(
  ({ variant }) => css`
    ${variant == "primary" && buttonStyles.primary}
    ${variant == "secondary" && buttonStyles.secondary}
  `
);

*/