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

import React from 'react';
import Card from "@mui/material/Card";
import Divider from "@mui/material/Divider";
import Icon from "@mui/material/Icon";
import { Box } from '@mui/material';
import Typography from '@mui/material/Typography';
import Invoice from './invoice';

import AccountBalanceIcon from '@mui/icons-material/AccountBalance';
import InfoIcon from '@mui/icons-material/Info';

interface ComplexProjectCardProps {
  color?: 'primary' | 'secondary' | 'info' | 'success' | 'warning' | 'error' | 'dark' | 'light';
  title: string;
  dateTime?: string;
  description: string;

  balance?: string;
}

const ComplexProjectCard: React.FC<ComplexProjectCardProps> = ({
  color = 'dark',
  title,
  dateTime = '',
  description,
  balance,
}) => {

  return (
    <Card sx={{ overflow: "visible" }}>
      <Box p={2}>
        <Box display="flex" alignItems="center">
          <Icon
            fontSize="large"
            sx={{
              width: "75px",
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              height: "74px",
              flexShrink: 0,
              backgroundImage: `linear-gradient(180deg, #3E3D45 0%, #202020 100%)`,
              borderRadius: "12px",
              color: "white",
              mt: -6,
            }}
          >
            <AccountBalanceIcon/>
          </Icon>
          <Box ml={2} mt={-2} lineHeight={0}>
            <Typography
              variant="h6"
              textTransform="capitalize"
              fontWeight="medium"
            >
              {title}
            </Typography>
          </Box>
          {/* {dropdown && (
            <Typography
              color="success"
              onClick={typeof dropdown !== 'boolean' ? dropdown.action : undefined}
              sx={{
                ml: "auto",
                mt: -1,
                alignSelf: "flex-start",
                py: 1.25,
              }}
            >
              <Icon
                sx={{ cursor: "pointer", fontWeight: "bold" }}
              >
                more_vert
              </Icon>
            </Typography>
          )} */}
          {/* {typeof dropdown !== 'boolean' && dropdown.menu} */}
        </Box>
        <Divider />
        <Divider />
        <Invoice name="Loan 1" date="2022/02/04" price="$20,000" />
        <Divider />
        <Invoice name="Loan 1" date="2022/02/04" price="$20,000" />
        <Divider />
        <Invoice name="Loan 1" date="2022/02/04" price="$20,000" />
        <Divider />
        <Box display="flex" justifyContent="flex-end" alignItems="center">
          {balance && (
            <Box
              display="flex"
              flexDirection="column"
              lineHeight={0}
              alignItems="center"
            >
              <Typography variant="h6" fontWeight="bold" color="dark">
                Balance
              </Typography>
              <Typography variant="button" fontWeight="medium">
                {balance}
              </Typography>
            </Box>
          )}
        </Box>
      </Box>
    </Card>
  );
};

export default ComplexProjectCard;

