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

// prop-types is a library for typechecking of props
import PropTypes from "prop-types";

// @mui material components
import Icon from "@mui/material/Icon";

// Material Dashboard 2 PRO React components
import {Box}from "@mui/material";
import Typography from "@mui/material/Typography";
import { IconButton } from "@mui/material";
import InfoIcon from '@mui/icons-material/Info';
interface invoiceProps{
    date?:string;
    id?:string;
    price:string;
    noGutter?:boolean
    name?:string;
}
function Invoice({ name,date, id, price, noGutter }:invoiceProps) {
  return (
    <Box
      component="li"
      display="flex"
      justifyContent="space-between"
      alignItems="center"
      py={1}
      pr={1}
      mb={noGutter ? 0 : 1}
    >
      <Box lineHeight={1.125}>
        <Typography display="block" variant="button" fontWeight="medium">
          {date}
        </Typography>
        <Typography variant="caption" fontWeight="regular" color="text">
          {id}
        </Typography>
      </Box>
      <Box display="flex" alignItems="center">
        <Typography variant="button" fontWeight="regular" color="text">
          {price}
        </Typography>
        <Box display="flex" alignItems="center" justifyContent="center" sx={{ cursor: "pointer" }}>
        <IconButton size="small" aria-label="close" color="info" >
            <InfoIcon fontSize="small"/>
          </IconButton>
          {/* <IconButton size="small" aria-label="close" color="inherit" onClick={close}>
          <Icon fontSize="small">close</Icon>
        </IconButton> */}
          <Typography variant="button" fontWeight="light">
            &nbsp;Details
          </Typography>
        </Box>
      </Box>
    </Box>
  );
}

// Setting default values for the props of Invoice
Invoice.defaultProps = {
  noGutter: false,
};

// Typechecking props for the Invoice
Invoice.propTypes = {
  date: PropTypes.string.isRequired,
  id: PropTypes.string.isRequired,
  price: PropTypes.string.isRequired,
  noGutter: PropTypes.bool,
};

export default Invoice;
