import styled, { css } from "styled-components";
import { HTMLImageProps } from "../../types";
import { ImageProps } from "./Image";

export const Image = styled(({transform,origin,...props}:ImageProps)=>(
    <img {...props}/>
))`
${({transform,origin})=> transform && origin && `transform:rotate(${transform}deg) ;

transform-origin:${origin};
`}
`

