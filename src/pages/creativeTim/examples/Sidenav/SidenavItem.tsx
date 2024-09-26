import React from 'react';
import Collapse from "@mui/material/Collapse";
import ListItem from "@mui/material/ListItem";
import ListItemText from "@mui/material/ListItemText";
import Icon from "@mui/material/Icon";
import Box from "@mui/material/Box";
import { Theme } from '@mui/material/styles';

// Assuming this is a custom hook defined elsewhere in your project
import { useMaterialUIController } from "../../context";

type ColorType = "primary" | "secondary" | "info" | "success" | "warning" | "error" | "dark";

interface SidenavItemProps {
  color?: ColorType;
  name: string;
  active?: boolean | string;
  nested?: boolean;
  children?: React.ReactNode;
  open?: boolean;
  [key: string]: any; // for ...rest props
}

const SidenavItem: React.FC<SidenavItemProps> = ({ 
  color = "info", 
  name, 
  active = false, 
  nested = false, 
  children = null, 
  open = false, 
  ...rest 
}) => {
  const [controller] = useMaterialUIController();
  const { miniSidenav, transparentSidenav, whiteSidenav, darkMode } = controller;

  return (
    <>
      <ListItem
        component="li"
        sx={{
          mb: 0.5,
          py: 1,
          pl: 3,
          color: (theme: Theme) => theme.palette.text.primary,
          '&:hover': {
            backgroundColor: (theme: Theme) => theme.palette.action.hover,
          },
        }}
      >
        <Box
          sx={{
            width: '100%',
            display: 'flex',
            alignItems: 'center',
            justifyContent: 'space-between',
          }}
        >
          <ListItemText 
            primary={name}
            primaryTypographyProps={{
              variant: 'body2',
              fontWeight: active ? 'bold' : 'normal',
              color: 'inherit',
            }}
          />
          {children && (
            <Icon
              sx={{
                fontWeight: 'normal',
                verticalAlign: 'middle',
                mr: 1,
                transform: open ? 'rotate(180deg)' : 'none',
                transition: "all 0.5s ease-in"
                ,
              }}
            >
              expand_less
            </Icon>
          )}
        </Box>
      </ListItem>
      {children && (
        <Collapse in={open} timeout="auto" unmountOnExit {...rest}>
          <Box sx={{ pl: nested ? 2 : 0 }}>
            {children}
          </Box>
        </Collapse>
      )}
    </>
  );
}

export default SidenavItem;