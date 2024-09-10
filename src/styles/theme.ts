import { DefaultTheme } from "styled-components";
import {typography} from "./commonstyles"

export const theme : DefaultTheme = {
    colors: {
        
    },
    typography,
    gradients:{
    purpleToBlue :`background: linear-gradient(90deg, #F67BF9 0%, #B57BEF 50%, #9EC5FF 100%);
    background-clip: text;-webkit-background-clip: text;
    -webkit-text-fill-color: transparent;font-feature-settings: 'cv12' on, 'cv13' on;`
    },
    breakpoint:{

    },
}