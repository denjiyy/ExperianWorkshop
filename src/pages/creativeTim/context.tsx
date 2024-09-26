/**
=========================================================
* Material Dashboard 2 PRO React - v2.1.0
=========================================================

* Product Page: https://www.creative-tim.com/product/material-dashboard-pro-react
* Copyright 2022 Creative Tim (https://www.creative-tim.com)

Coded by www.creative-tim.com

 =========================================================

* The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
*/

/**
  This file is used for controlling the global states of the components,
  you can customize the states for the different components here.
*/

import { createContext, useContext, useMemo, useReducer, ReactNode, Dispatch } from "react";

// prop-types is a library for typechecking of props
import PropTypes from "prop-types";

interface State {
  miniSidenav: boolean;
  transparentSidenav: boolean;
  whiteSidenav: boolean;
  sidenavColor:"primary" | "secondary" | "error" | "info" | "success" | "warning" | "dark";
  transparentNavbar: boolean;
  fixedNavbar: boolean;
  openConfigurator: boolean;
  direction: string;
  layout: string;
  darkMode: boolean;
}

type Action =
  | { type: "MINI_SIDENAV"; value: boolean }
  | { type: "TRANSPARENT_SIDENAV"; value: boolean }
  | { type: "WHITE_SIDENAV"; value: boolean }
  | { type: "SIDENAV_COLOR"; value: "primary" | "secondary" | "error" | "info" | "success" | "warning" | "dark" }
  | { type: "TRANSPARENT_NAVBAR"; value: boolean }
  | { type: "FIXED_NAVBAR"; value: boolean }
  | { type: "OPEN_CONFIGURATOR"; value: boolean }
  | { type: "DIRECTION"; value: string }
  | { type: "LAYOUT"; value: string }
  | { type: "DARKMODE"; value: boolean };

// The Material Dashboard 2 PRO React main context
const MaterialUI = createContext<[State, Dispatch<Action>] | undefined>(undefined);

// Setting custom name for the context which is visible on react dev tools
MaterialUI.displayName = "MaterialUIContext";

// Material Dashboard 2 PRO React reducer
function reducer(state: State, action: Action): State {
  switch (action.type) {
    case "MINI_SIDENAV":
      return { ...state, miniSidenav: action.value };
    case "TRANSPARENT_SIDENAV":
      return { ...state, transparentSidenav: action.value };
    case "WHITE_SIDENAV":
      return { ...state, whiteSidenav: action.value };
    case "SIDENAV_COLOR":
      return { ...state, sidenavColor: action.value };
    case "TRANSPARENT_NAVBAR":
      return { ...state, transparentNavbar: action.value };
    case "FIXED_NAVBAR":
      return { ...state, fixedNavbar: action.value };
    case "OPEN_CONFIGURATOR":
      return { ...state, openConfigurator: action.value };
    case "DIRECTION":
      return { ...state, direction: action.value };
    case "LAYOUT":
      return { ...state, layout: action.value };
    case "DARKMODE":
      return { ...state, darkMode: action.value };
    default:
      throw new Error(`Unhandled action type: ${(action as Action).type}`);
  }
}

// Material Dashboard 2 PRO React context provider
function MaterialUIControllerProvider({ children }: { children: ReactNode }) {
  const initialState: State = {
    miniSidenav: false,
    transparentSidenav: false,
    whiteSidenav: false,
    sidenavColor: "primary",
    transparentNavbar: true,
    fixedNavbar: true,
    openConfigurator: false,
    direction: "ltr",
    layout: "dashboard",
    darkMode: false,
  };

  const [controller, dispatch] = useReducer(reducer, initialState);
  const value = useMemo(() => [controller, dispatch] as [State, Dispatch<Action>], [controller, dispatch]);

  return <MaterialUI.Provider value={value}>{children}</MaterialUI.Provider>;
}

// Material Dashboard 2 PRO React custom hook for using context
function useMaterialUIController(): [State, Dispatch<Action>] {
  const context = useContext(MaterialUI);

  if (!context) {
    throw new Error(
      "useMaterialUIController should be used inside the MaterialUIControllerProvider."
    );
  }

  return context;
}

// Typechecking props for the MaterialUIControllerProvider
MaterialUIControllerProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

// Context module functions
const setMiniSidenav = (dispatch: Dispatch<Action>, value: boolean) => dispatch({ type: "MINI_SIDENAV", value });
const setTransparentSidenav = (dispatch: Dispatch<Action>, value: boolean) => dispatch({ type: "TRANSPARENT_SIDENAV", value });
const setWhiteSidenav = (dispatch: Dispatch<Action>, value: boolean) => dispatch({ type: "WHITE_SIDENAV", value });
const setSidenavColor = (dispatch: Dispatch<Action>, value: "primary" | "secondary" | "error" | "info" | "success" | "warning" | "dark") => dispatch({ type: "SIDENAV_COLOR", value });
const setTransparentNavbar = (dispatch: Dispatch<Action>, value: boolean) => dispatch({ type: "TRANSPARENT_NAVBAR", value });
const setFixedNavbar = (dispatch: Dispatch<Action>, value: boolean) => dispatch({ type: "FIXED_NAVBAR", value });
const setOpenConfigurator = (dispatch: Dispatch<Action>, value: boolean) => dispatch({ type: "OPEN_CONFIGURATOR", value });
const setDirection = (dispatch: Dispatch<Action>, value: string) => dispatch({ type: "DIRECTION", value });
const setLayout = (dispatch: Dispatch<Action>, value: string) => dispatch({ type: "LAYOUT", value });
const setDarkMode = (dispatch: Dispatch<Action>, value: boolean) => dispatch({ type: "DARKMODE", value });

export {
  MaterialUIControllerProvider,
  useMaterialUIController,
  setMiniSidenav,
  setTransparentSidenav,
  setWhiteSidenav,
  setSidenavColor,
  setTransparentNavbar,
  setFixedNavbar,
  setOpenConfigurator,
  setDirection,
  setLayout,
  setDarkMode,
};

