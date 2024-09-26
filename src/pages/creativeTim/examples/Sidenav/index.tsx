import React, { useEffect, useState } from "react";
import { useLocation, NavLink } from "react-router-dom";
import {
  List,
  Divider,
  Link,
  Icon,
  Box,
  Typography,
  Theme,
  useTheme,
} from "@mui/material";
import SidenavCollapse from "./SidenavCollapse";
import SidenavList from "./SidenavList";
import SidenavItem from "./SidenavItem";
import {
  useMaterialUIController,
  setMiniSidenav,
  setTransparentSidenav,
  setWhiteSidenav,
} from "../../context";
import SidenavDrawer from "./SidenavRoot";

interface RouteType {
  type: string;
  name?: string;
  icon?: React.ReactNode;
  title?: string;
  noCollapse?: boolean;
  key: string;
  href?: string;
  route?: string;
}


interface SidenavProps {
  color: "primary" | "secondary" | "info" | "success" | "warning" | "error" | "dark";
  brandName: string;
  routes: RouteType[];
}

const Sidenav: React.FC<SidenavProps> = ({ color, brandName, routes }) => {
  const [openCollapse, setOpenCollapse] = useState<string | false>(false);
  const [openNestedCollapse, setOpenNestedCollapse] = useState<string | false>(false);
  const [controller, dispatch] = useMaterialUIController();
  const { miniSidenav, transparentSidenav, whiteSidenav, darkMode } = controller;
  const location = useLocation();
  const { pathname } = location;
  const collapseName = pathname.split("/").slice(1)[0];
  const items = pathname.split("/").slice(1);
  const itemParentName = items[1];
  const itemName = items[items.length - 1];

  const theme = useTheme();

  let textColor: "white" | "dark" | "inherit" = "white";

  if (transparentSidenav || (whiteSidenav && !darkMode)) {
    textColor = "dark";
  } else if (whiteSidenav && darkMode) {
    textColor = "inherit";
  }

  const closeSidenav = () => setMiniSidenav(dispatch, true);

  useEffect(() => {
    setOpenCollapse(collapseName);
    setOpenNestedCollapse(itemParentName);
  }, [collapseName, itemParentName]);

  useEffect(() => {
    const handleMiniSidenav = () => {
      setMiniSidenav(dispatch, window.innerWidth < 1200);
      setTransparentSidenav(dispatch, window.innerWidth < 1200 ? false : transparentSidenav);
      setWhiteSidenav(dispatch, window.innerWidth < 1200 ? false : whiteSidenav);
    };

    window.addEventListener("resize", handleMiniSidenav);
    handleMiniSidenav();

    return () => window.removeEventListener("resize", handleMiniSidenav);
  }, [dispatch, location, transparentSidenav, whiteSidenav]);



 

  const renderRoutes = routes.map(
    ({ type, name, icon, title, noCollapse, key, href, route }) => {
      let returnValue;

      switch (type) {
        case "title":
          returnValue = (
            <Typography
              key={key}
              color={textColor}
              display="block"
              variant="caption"
              fontWeight="bold"
              textTransform="uppercase"
              pl={3}
              mt={2}
              mb={1}
              ml={1}
            >
              {title}
            </Typography>
          );
          break;
        case "divider":
          returnValue = (
            <Divider
              key={key}
            sx={{opacity:0.6}}
            />
          );
          break;
        case "nocollapse":
          if(name){
          returnValue = (
            <NavLink to={route || ""} key={key}>
              <SidenavCollapse
                name={name}
                icon={icon}
                noCollapse={noCollapse}
                active={key === collapseName}
              />
            </NavLink>
          );
        }
          break;
      }
      return returnValue;
    }
  );

  const profilePictureRoutes = routes
    .filter(({ type }) => type === "profile")
    .map(({ key, name, icon, href, route, noCollapse }) => {
      let returnValue;
      if(name){
      if (href) {
        returnValue = (
          <Link
            href={href}
            key={key}
            target="_blank"
            rel="noreferrer"
            sx={{ textDecoration: "none" }}
          >
            <SidenavCollapse
              name={name}
              icon={icon}
              active={key === collapseName}
              noCollapse={noCollapse}
            />
          </Link>
        );
      } else if (noCollapse && route) {
        returnValue = (
          <NavLink to={route} key={key}>
            <SidenavCollapse
              name={name}
              icon={icon}
              noCollapse={noCollapse}
              active={key === collapseName}
            >
            </SidenavCollapse>
          </NavLink>
        );
      } else {
        returnValue = (
          <SidenavCollapse
            key={key}
            name={name}
            icon={icon}
            active={key === collapseName}
            open={openCollapse === key}
            onClick={() =>
              openCollapse === key ? setOpenCollapse(false) : setOpenCollapse(key)
            }
          >
          </SidenavCollapse>
        );
      }
    }
      return returnValue;
    });

  return (
    <Box>
      <Box pt={3} pb={1} px={4} textAlign="center">
        <Box
          display={{ xs: "block", xl: "none" }}
          position="absolute"
          top={0}
          right={0}
          p={1.625}
          onClick={closeSidenav}
          sx={{ cursor: "pointer" }}
        >
          <Typography variant="h6" color="secondary">
            <Icon sx={{ fontWeight: "bold" }}>close</Icon>
          </Typography>
        </Box>
        <Box component={NavLink} to="/" display="flex" alignItems="center">
          <Box>
            <Typography component="h6" variant="button" fontWeight="medium" color={textColor}>
              {brandName}
            </Typography>
          </Box>
        </Box>
      </Box>
      <Divider sx={{ opacity: 0.6 }} />
      <List sx={{
        display: "flex",
        width: "100%",
        flexDirection: "column",
        gap: "11px",
        flex: "1 0 0",
        pb: 2
      }}>
        {renderRoutes}
        <Box sx={{ mt: "auto" }}>
          {profilePictureRoutes}
        </Box>
      </List>
    </Box>
  );
};

export default Sidenav;