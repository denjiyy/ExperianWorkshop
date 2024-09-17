import * as S from "./elements"
import { HTMLHeadingPropsH1,HTMLHeadingPropsH3,HTMLHeadingPropsH4,HTMLHeadingPropsBody, HTMLSpanProps} from "../../types";
export interface H1Props extends HTMLHeadingPropsH1 {
    variant?: "bold" | "medium",
    gradient?: "none" | "purpleToBlue",
  }
  
  export const H1Bd = ({ variant = "bold",gradient="none", ...props }: H1Props) => {
    return <S.Heading1 {...props} variant={variant} gradient={gradient} />;
  };
  export const H1Md = ({ variant = "medium",gradient="none", ...props }: H1Props) => {
    return <S.Heading1 gradient={gradient} {...props} variant={variant} />;
  };

  
  
  export interface H3Props extends HTMLHeadingPropsH3 {
    variant?: "bold";
  }
  
  export const H3Bd = ({ variant = "bold", ...props }: H3Props) => {
    return <S.Heading3 {...props} variant={variant} />;
  };
  
  export interface H4Props extends HTMLHeadingPropsH4 {
    variant?: "medium";
  }
  
  export const H4Md = ({ variant = "medium", ...props }: H4Props) => {
    return <S.Heading4 {...props} variant={variant} />;
  };

  export interface BodyProps extends HTMLHeadingPropsBody{
    variant : "large" | "medium"
    subvariant : "none" | "medium" | "regular" | "semibold"
  }
  export const BodyLg = ({variant="large",subvariant="none",...props}:BodyProps)=>{
    return <S.Body {...props} variant={variant} subvariant={subvariant}/>
  } 
  BodyLg.defaultProps = {
    variant: "large",
    subvariant: "none"
};

  export const BodyMdMd = ({variant="medium",subvariant="medium",...props}:BodyProps)=>{
    return <S.Body {...props} variant={variant} subvariant={subvariant}/>
  } 
  BodyMdMd.defaultProps = {
    variant: "medium",
    subvariant: "medium"
};
  export const BodyMdRg = ({variant="medium",subvariant="regular",...props}:BodyProps)=>{
    return <S.Body {...props} variant={variant} subvariant={subvariant}/>
    
  } 
  BodyMdRg.defaultProps = {
    variant: "medium",
    subvariant: "regular"
};
  export const BodyMdSb = ({variant="medium",subvariant="semibold",...props}:BodyProps)=>{
    return <S.Body {...props} variant={variant} subvariant={subvariant}/>
  }
  BodyMdSb.defaultProps = {
    variant: "medium",
    subvariant: "semibold"
}; 

export interface SpanProps extends HTMLSpanProps{
  gradient : "none" | "purpleToBlue",
}

export const SpanElement = ({gradient="none",...props}:SpanProps)=>{
return <S.Span {...props} gradient={gradient}/>
}


  // export interface BodyMdProps extends BodyProps{
  //   subvariant : "medium" | "regular" | "semibold"
  // }
  // export const BodyMdMd = ({variant="medium", subvariant="medium",...props}:BodyMdProps)=>
  // {
  //   return <S.BodyMd {...props} variant={variant} subvariant={subvariant}/>
  // }

  // export const BodyMdRg = ({variant="medium", subvariant="regular",...props}:BodyMdProps)=>
  //   {
  //     return <S.BodyMd {...props} variant={variant} subvariant={subvariant}/>
  //   }
    
  //   export const BodyMdSb = ({variant="medium", subvariant="semibold",...props}:BodyMdProps)=>
  //     {
  //       return <S.BodyMd {...props} variant={variant} subvariant={subvariant}/>
  //     }

  
  // export interface H5Props extends HTMLHeadingProps {
  //   variant?: "regular";
  // }
  
  // export const H5 = ({ variant = "regular", ...props }: H5Props) => {
  //   return <S.Heading5 {...props} variant={variant} />;
  // };
  
  // export interface H6Props extends HTMLHeadingProps {
  //   variant?: "regular";
  // }
  
  // export const H6 = ({ variant = "regular", ...props }: H6Props) => {
  //   return <S.Heading6 {...props} variant={variant} />;
  // };
  
  // export interface ParagraphProps extends HTMLHeadingProps {
  //   variant?: "regular";
  // }
  
  // export const Paragraph = ({
  //   variant = "regular",
  //   ...props
  // }: ParagraphProps) => {
  //   return <S.Paragraph {...props} variant={variant} />;
  // };
  
  export const Typography = {
    H1Bd,
    H1Md,
    H3Bd,
    H4Md,
    BodyLg,
    BodyMdMd,
    BodyMdRg,
    BodyMdSb
  };
  