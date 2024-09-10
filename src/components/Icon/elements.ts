import styled, { css } from "styled-components";
import { HTMLSVGProps } from "../../types";
const IconStyles=css`
width: 20px;
height: 20px;

`
export const Icon = styled.svg<HTMLSVGProps>(() => css`${IconStyles}`)

