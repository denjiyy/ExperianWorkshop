import React from 'react';
import Collapse from "@mui/material/Collapse";
import ListItem from "@mui/material/ListItem";
import ListItemIcon from "@mui/material/ListItemIcon";
import ListItemText from "@mui/material/ListItemText";
import Icon from "@mui/material/Icon";
import Box from "@mui/material/Box";
import { Theme } from '@mui/material/styles';

// Assuming these are custom hooks and styles defined elsewhere in your project
import { useMaterialUIController } from "../../context";
import {
  collapseItem
} from "./styles/sidenavCollapse";

interface SidenavCollapseProps {
  icon: React.ReactNode;
  name: string;
  children?: React.ReactNode;
  active?: boolean;
  noCollapse?: boolean;
  open?: boolean;
  [key: string]: any; // for ...rest props
}

const SidenavCollapse: React.FC<SidenavCollapseProps> = ({ 
  icon, 
  name, 
  children, 
  active = false, 
  noCollapse = false,
  open = false,
  ...rest 
}) => {
  const [controller] = useMaterialUIController();
  const { miniSidenav, transparentSidenav, whiteSidenav, darkMode } = controller;

  return (
    <>
      <ListItem component="li">
        <Box
          {...rest}
          sx={(theme: Theme) =>
            collapseItem()
          }
        >
          <ListItemIcon
          >
     
          </ListItemIcon>

          <ListItemText
            primary={name}
            
          />

          <Icon
        
          >
            expand_less
          </Icon>
        </Box>
      </ListItem>
      {children && (
        <Collapse in={open} unmountOnExit>
          {children}
        </Collapse>
      )}
    </>
  );
}

export default SidenavCollapse;