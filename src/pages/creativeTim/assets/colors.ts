export interface Color {
  main: string;
  focus?: string;
  state?: string;
  dark?: string;
  light?: string;
  background?: string;
  text?: string;
}

export  interface GradientColor {
  main: string;
  state: string;
}

 export interface ColorsProps {
  background: {
    default: string;
  };
  text: Color;
  transparent: Color;
  white: Color;
  black: Color;
  primary: Color;
  secondary: Color;
  info: Color;
  success: Color;
  warning: Color;
  error: Color;
  light: Color;
  dark: Color;
  grey: {
    [key: number]: string;
  };
  gradients: {
    primary: GradientColor;
    secondary: GradientColor;
    info: GradientColor;
    success: GradientColor;
    warning: GradientColor;
    error: GradientColor;
    light: GradientColor;
    dark: GradientColor;
  };
 
}
export type ColorKey =  "primary" | "secondary" | "info" | "success" | "warning" | "error" | "light" | "dark" | "transparent" | "white" | "black" | "grey" ;
const colors: ColorsProps = {
  background: {
    default: "#f0f2f5",
  },

  text: {
    main: "#7b809a",
    focus: "#7b809a",
  },

  transparent: {
    main: "transparent",
  },

  white: {
    main: "#ffffff",
    focus: "#ffffff",
  },

  black: {
    light: "#000000",
    main: "#000000",
    focus: "#000000",
  },

  primary: {
    main: "#e91e63",
    focus: "#e91e63",
  },

  secondary: {
    main: "#7b809a",
    focus: "#8f93a9",
  },

  info: {
    main: "#1A73E8",
    focus: "#1662C4",
  },

  success: {
    main: "#4CAF50",
    focus: "#67bb6a",
  },

  warning: {
    main: "#fb8c00",
    focus: "#fc9d26",
  },

  error: {
    main: "#F44335",
    focus: "#f65f53",
  },

  light: {
    main: "#f0f2f5",
    focus: "#f0f2f5",
  },

  dark: {
    main: "#344767",
    focus: "#2c3c58",
  },

  grey: {
    100: "#f8f9fa",
    200: "#f0f2f5",
    300: "#dee2e6",
    400: "#ced4da",
    500: "#adb5bd",
    600: "#6c757d",
    700: "#495057",
    800: "#343a40",
    900: "#212529",
  },

  gradients: {
    primary: {
      main: "#EC407A",
      state: "#D81B60",
    },

    secondary: {
      main: "#747b8a",
      state: "#495361",
    },

    info: {
      main: "#49a3f1",
      state: "#1A73E8",
    },

    success: {
      main: "#66BB6A",
      state: "#43A047",
    },

    warning: {
      main: "#FFA726",
      state: "#FB8C00",
    },

    error: {
      main: "#EF5350",
      state: "#E53935",
    },

    light: {
      main: "#EBEFF4",
      state: "#CED4DA",
    },

    dark: {
      main: "#42424a",
      state: "#191919",
    },
  },

  
};

export default colors;

