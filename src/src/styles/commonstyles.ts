import styled,{ css,Interpolation,DefaultTheme } from "styled-components";

type ThemeProps<T> = {
    theme: T;
};
export type FlattenInterpolation<P> = ReadonlyArray<Interpolation<ThemeProps<P>>>;
export interface TypographyStylesProps {
//   h1: { regular: FlattenInterpolation<ThemeProps<DefaultTheme>> };
//   h2: { regular: FlattenInterpolation<ThemeProps<DefaultTheme>> };
//   h3: { regular: FlattenInterpolation<ThemeProps<DefaultTheme>> };
//   h4: { regular: FlattenInterpolation<ThemeProps<DefaultTheme>> };
//   h5: { regular: FlattenInterpolation<ThemeProps<DefaultTheme>> };
//   h6: { regular: FlattenInterpolation<ThemeProps<DefaultTheme>> };
//   p: { regular: FlattenInterpolation<ThemeProps<DefaultTheme>> };
h1:{
    bold : FlattenInterpolation<DefaultTheme> , 
    medium : FlattenInterpolation<DefaultTheme>
}
h3:{
    bold:FlattenInterpolation<DefaultTheme>,
}
h4:{
    medium: FlattenInterpolation<DefaultTheme>,
}
body:{
 large:FlattenInterpolation<DefaultTheme>,
    medium:{
        medium:FlattenInterpolation<DefaultTheme>,
        regular:FlattenInterpolation<DefaultTheme>,
        semibold:FlattenInterpolation<DefaultTheme>
    }
}
}



export const typography: TypographyStylesProps = {
//   h1: {
//     regular: css``
//   },
//   h2: {
//     regular: css``
//   },
//   h3: {
//     regular: css``
//   },
//   h4: {
//     regular: css``
//   },
//   h5: {
//     regular: css``
//   },
//   h6: {
//     regular: css``
//   },
//   p: {
//     regular: css``
//   }
h1:{
    bold: css`
font-feature-settings: 'cv12' on, 'cv13' on;
 font-size: 70px;
font-style: normal;
font-weight: 700;
line-height: 100%;
letter-spacing: -2.1px;
    `,
    medium:css`
font-feature-settings: 'cv12' on, 'cv13' on;
/* Headline/H1 Medium */
 font-size: 70px;
font-style: normal;
font-weight: 500;
line-height: 100%; /* 70px */
letter-spacing: -2.1px;`,
},
h3:{
    bold:css`
    font-size:40px;
    font-style:normal;
    font-weight:700;
    line-height:100%;
    letter-spacing:-2.1px
    `,
},
h4:{
    medium:css`
/* Headline/H4 Medium */
font-size: 24px;
font-style: normal;
font-weight: 500;
line-height: 32px; /* 133.333% */
letter-spacing: -0.48px;`
},
body:{
    large:css`
font-feature-settings: 'cv12' on, 'cv13' on;

/* Body/Lg Medium */
font-size: 18px;
font-style: normal;
font-weight: 500;
line-height: 26px; /* 144.444% */
letter-spacing: -0.36px;`,
    medium:{
        medium:css`

/* Body/Md Medium */
font-size: 14px;
font-style: normal;
font-weight: 500;
line-height: 20px; /* 142.857% */
letter-spacing: -0.42px;`,
        regular:css`
font-family: Inter;
font-size: 14px;
font-style: normal;
font-weight: 400;
line-height: 20px; /* 142.857% */
letter-spacing: -0.28px;`,
        semibold:css`
font-size: 14px;
font-style: normal;
font-weight: 600;
line-height: 20px; /* 142.857% */
letter-spacing: -0.28px;`,
    }
}
};

//const StyledH1Medium = styled.h1`${typography.h1.medium}`;
