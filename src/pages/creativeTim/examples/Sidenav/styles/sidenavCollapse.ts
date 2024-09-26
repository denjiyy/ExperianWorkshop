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
import { borderRadius } from "../../../assets/borderRadius";
import { theme } from "../../../../../styles/"
import { grey } from "@mui/material/colors";
function collapseItem() {

  return {
    background: grey[300],
    color: grey[900],
    display: "flex",
    alignItems: "center",
    width: "100%",
    padding: `0.5rem 1rem`,
    margin: `0,093475 1rem`,
    borderRadius: borderRadius.md,
    cursor: "pointer",
    userSelect: "none",
    whiteSpace: "nowrap",
    boxShadow: "none",

    "&:hover, &:focus": {
      backgroundColor:grey[300]
    },
  };
}



export { collapseItem };
