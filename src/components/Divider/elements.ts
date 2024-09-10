import styled, { css } from "styled-components";
import { BodyMdSb as _body } from "../Typography";
import { DividerProps } from "./Divider";
export const DividerText = styled(_body)<DividerProps>`
  font-weight:400;
  display: grid;
  grid-template-columns: 1fr auto 1fr; /* Adjust grid to place text in the center */
  align-items: center;
column-gap: ${({ text }) => (text==="none" ? `0rem` : `0.5rem`)};
  color: #14151A;
  opacity:0.75;
  &::before,
  &::after {
    content: "";
    height: 1.5px;
    background-color: #14151A;
    display: block;
    opacity: 0.75;
  }

  &::before {
    grid-column: 1; /* Place before pseudo-element in the first column */
  }

  &::after {
    grid-column: 3; /* Place after pseudo-element in the third column */
  }
`