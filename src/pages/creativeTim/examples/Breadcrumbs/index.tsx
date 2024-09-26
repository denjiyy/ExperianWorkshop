
import React from "react";
import { Link } from "react-router-dom";
import { Breadcrumbs as MuiBreadcrumbs, Box, Typography } from "@mui/material";
import Icon from "@mui/material/Icon";
import { grey } from "@mui/material/colors";

interface BreadcrumbsProps {
  icon: React.ReactNode;
  title: string;
  route: string[];
  light?: boolean;
}

function Breadcrumbs({ icon, title, route, light = false }: BreadcrumbsProps) {
  return (
    <Box mr={{ xs: 0, xl: 8 }}>
      <MuiBreadcrumbs
        sx={{
          "& .MuiBreadcrumbs-separator": {
            color: grey[600],
          },
        }}
      >
        <Link to="/">
          <Typography
            component="span"
            variant="body2"
            color="textSecondary"
            sx={{ lineHeight: 0, opacity: 0.8 }}
          >
            <Icon>{icon}</Icon>
          </Typography>
        </Link>
        {route.map((el: string) => (
          <Link to={`/${el}`} key={el}>
            <Typography
              component="span"
              variant="button"
              fontWeight="regular"
              textTransform="capitalize"
              color="textSecondary"
              sx={{ lineHeight: 0, opacity: 0.5 }}
            >
              {el}
            </Typography>
          </Link>
        ))}
        <Typography
          variant="button"
          fontWeight="regular"
          textTransform="capitalize"
          color="textPrimary"
          sx={{ lineHeight: 0 }}
        >
          {title.replace("-", " ")}
        </Typography>
      </MuiBreadcrumbs>
      <Typography
        fontWeight="bold"
        textTransform="capitalize"
        variant="h6"
        color="textPrimary"
        noWrap
      >
        {title.replace("-", " ")}
      </Typography>
    </Box>
  );
}

export default Breadcrumbs;
