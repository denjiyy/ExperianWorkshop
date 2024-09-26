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

import { useState, useEffect } from "react";

// prop-types is a library for typechecking of props
import PropTypes from "prop-types";

// @mui material components
import { Grid2 as Grid } from "@mui/material";
import Box from "@mui/material/Box";
import AppBar from "@mui/material/AppBar";
import Tabs from "@mui/material/Tabs";
import Tab from "@mui/material/Tab";

// Material Dashboard 2 PRO React components

// Material Dashboard 2 PRO React base styles

// Material Dashboard 2 PRO React examples
import DashboardLayout from "../../readySections/dashBoardLayout";
import DashboardNavbar from "../../readySections/dashBoardNavbar";
import breakpoints from "../../assets/breakpoints";
import { ChildrenProps } from "@/src/types/children";
function BaseLayout({ children }:ChildrenProps) {
  const [tabsOrientation, setTabsOrientation] = useState("horizontal");
  const [tabValue, setTabValue] = useState(0);

  useEffect(() => {
    // A function that sets the orientation state of the tabs.
    function handleTabsOrientation() {
      return window.innerWidth < breakpoints.values.sm
        ? setTabsOrientation("vertical")
        : setTabsOrientation("horizontal");
    }

    /** 
     The event listener that's calling the handleTabsOrientation function when resizing the window.
    */
    window.addEventListener("resize", handleTabsOrientation);

    // Call the handleTabsOrientation function to set the state with the initial value.
    handleTabsOrientation();

    // Remove event listener on cleanup
    return () => window.removeEventListener("resize", handleTabsOrientation);
  }, [tabsOrientation]);


  return (
    <DashboardLayout>
      <DashboardNavbar />
      <Box  sx={{ mt: 3 }}>
        <Grid container>
          <Grid size={{xs:12,sm:8,lg:4}}>
            <AppBar position="static">
          
            </AppBar>
          </Grid>
        </Grid>
        {children}
      </Box>
    </DashboardLayout>
  );
}

// Setting default values for the props of BaseLayou

// Typechecking props for BaseLayout

export default BaseLayout;
