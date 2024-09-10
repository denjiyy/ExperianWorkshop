
import { HTMLImageProps } from '../../types';
import * as S from './elements';

export interface ImageProps extends HTMLImageProps{
  transform ?: number;
  origin ?: string;
}
export const Image = ({ ...props }: ImageProps) => {
  return <S.Image {...props} />;
};
