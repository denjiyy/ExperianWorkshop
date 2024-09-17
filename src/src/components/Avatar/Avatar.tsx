import { HTMLImageProps } from "../../types"
import * as S from "./elements"

export const Avatar = ({ src, ...props }: HTMLImageProps) => {
    return (
        <S.AvatarDiv>
            <S.AvatarImage src={src} />
        </S.AvatarDiv>
    );
}