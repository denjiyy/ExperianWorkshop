import { ReactNode } from "react";
import { BodyMdMd } from "../Typography";
import * as S from "./elements"
// export interface LinkProps extends
// interface NavLinkProps
//   extends Omit<
//     LinkProps,
//     "className" | "style" | "children"
//   > {
//   caseSensitive?: boolean;
//   children?:
//     | React.ReactNode
//     | ((props: { isActive: boolean }) => React.ReactNode);
//   className?:
//     | string
//     | ((props: {
//         isActive: boolean;
//       }) => string | undefined);
//   end?: boolean;
//   style?:
//     | React.CSSProperties
//     | ((props: {
//         isActive: boolean;
//       }) => React.CSSProperties);
// }
interface NavLinkProps extends Omit<React.AnchorHTMLAttributes<HTMLAnchorElement>, 'href'> {
    to: string | object;
    activeClassName?: string;
    activeStyle?: React.CSSProperties;
    exact?: boolean;
    isActive?: (match:string | null , location: Location) => boolean;
    end?: boolean;
}
export const Link = ({children,to,target,...props}:NavLinkProps)=>{
    <S.Link to={to} target={target} {...props}>
       {children} 
    </S.Link>
}