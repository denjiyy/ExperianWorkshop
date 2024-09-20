import Button from "@mui/material/Button";
import { styled } from "@mui/material/styles";

export default styled(Button)(({ theme, ownerState }) => {
  const { palette = {}, borders = {}, functions = {} } = theme;
  const { color = 'primary', variant, size, circular, iconOnly, darkMode } = ownerState;

  const { white = {}, text = {}, transparent = {}, gradients = {}, grey = {} } = palette;
  const { pxToRem, rgba, linearGradient } = functions;
  const { borderRadius } = borders;

  // Default box-shadow values
  const defaultBoxShadow = "0px 3px 1px -2px rgba(0, 0, 0, 0.2), 0px 2px 2px 0px rgba(0, 0, 0, 0.14), 0px 1px 5px 0px rgba(0, 0, 0, 0.12)";

  const containedStyles = () => {
    const backgroundValue = palette[color]?.main || white.main || '#fff';
    const focusedBackgroundValue = palette[color]?.focus || white.focus || '#fff';
    const boxShadowValue = defaultBoxShadow;
    const hoveredBoxShadowValue = defaultBoxShadow;
    let colorValue = white.main || '#fff';

    if (!darkMode && (color === "white" || color === "light" || !palette[color])) {
      colorValue = text.main || '#000';
    } else if (darkMode && (color === "white" || color === "light" || !palette[color])) {
      colorValue = grey[600] || '#ccc';
    }

    return {
      background: backgroundValue,
      color: colorValue,
      boxShadow: boxShadowValue,

      "&:hover": {
        backgroundColor: backgroundValue,
        boxShadow: hoveredBoxShadowValue,
      },

      "&:focus:not(:hover)": {
        backgroundColor: focusedBackgroundValue,
        boxShadow: defaultBoxShadow,
      },

      "&:disabled": {
        backgroundColor: backgroundValue,
        color: colorValue,
      },
    };
  };

  const outliedStyles = () => {
    const backgroundValue = color === "white" ? rgba(white.main, 0.1) : transparent.main || 'transparent';
    const colorValue = palette[color]?.main || white.main || '#fff';
    const borderColorValue = palette[color]?.main || rgba(white.main, 0.75) || '#ddd';

    return {
      background: backgroundValue,
      color: colorValue,
      borderColor: borderColorValue,

      "&:hover": {
        background: transparent.main || 'transparent',
        borderColor: colorValue,
      },

      "&:focus:not(:hover)": {
        background: transparent.main || 'transparent',
        boxShadow: defaultBoxShadow,
      },

      "&:disabled": {
        color: colorValue,
        borderColor: colorValue,
      },
    };
  };

  const gradientStyles = () => {
    const backgroundValue = color === "white" || !gradients[color]
      ? white.main || '#fff'
      : linearGradient(gradients[color]?.main, gradients[color]?.state);
    const colorValue = color === "white" ? text.main || '#000' : white.main || '#fff';

    return {
      background: backgroundValue,
      color: colorValue,
      boxShadow: defaultBoxShadow,

      "&:hover": {
        boxShadow: defaultBoxShadow,
      },

      "&:disabled": {
        background: backgroundValue,
        color: colorValue,
      },
    };
  };

  const textStyles = () => {
    const colorValue = palette[color]?.main || white.main || '#fff';
    const focusedColorValue = palette[color]?.focus || white.focus || '#fff';

    return {
      color: colorValue,

      "&:hover": {
        color: focusedColorValue,
      },

      "&:focus:not(:hover)": {
        color: focusedColorValue,
      },
    };
  };

  const circularStyles = () => ({
    borderRadius: borderRadius?.section || '50%',
  });

  const iconOnlyStyles = () => {
    let sizeValue = pxToRem(38) || '38px';

    if (size === "small") {
      sizeValue = pxToRem(25.4) || '25px';
    } else if (size === "large") {
      sizeValue = pxToRem(52) || '52px';
    }

    let paddingValue = `${pxToRem(11)} ${pxToRem(11)} ${pxToRem(10)}` || '11px 11px 10px';

    if (size === "small") {
      paddingValue = pxToRem(4.5) || '4.5px';
    } else if (size === "large") {
      paddingValue = pxToRem(16) || '16px';
    }

    return {
      width: sizeValue,
      minWidth: sizeValue,
      height: sizeValue,
      minHeight: sizeValue,
      padding: paddingValue,

      "& .material-icons": {
        marginTop: 0,
      },

      "&:hover, &:focus, &:active": {
        transform: "none",
      },
    };
  };

  return {
    ...(variant === "contained" && containedStyles()),
    ...(variant === "outlined" && outliedStyles()),
    ...(variant === "gradient" && gradientStyles()),
    ...(variant === "text" && textStyles()),
    ...(circular && circularStyles()),
    ...(iconOnly && iconOnlyStyles()),
  };
});
