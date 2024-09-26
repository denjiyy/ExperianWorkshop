import React from 'react';
import { Drawer, useTheme, Theme } from '@mui/material';
import {theme} from "../../../../styles"
import { DefaultTheme } from 'styled-components';
interface SidenavDrawerProps {
  miniSidenav: boolean;
  children: React.ReactNode;
}

const SidenavDrawer: React.FC<SidenavDrawerProps> = ({ 
  miniSidenav, 
  children 
}) => {

  return (
    <Drawer
      variant="permanent"
      sx={() => ({
        width: miniSidenav ? 96 : 320,
        transition: 'all 0.5s ease-in',
        overflowX: 'hidden',
        backgroundColor:'transparent',
        boxShadow: 'none' ,
        [theme.breakpoints.up('xl')]: {
          width: miniSidenav ? 96 : 320,
          marginBottom: 0 ,
          left: 0,
          transform: 'translateX(0)',
        },
        [theme.breakpoints.down('xl')]: {
          transform: miniSidenav ? 'translateX(-320px)' : 'translateX(0)',
        },
        '& .MuiDrawer-paper': {
          width: miniSidenav ? 96 : 320,
          transition: 'all 0.5s ease-in',
          overflowX: 'hidden',
          backgroundColor: 'transparent',
          boxShadow: 'none' ,
          [theme.breakpoints.up('xl')]: {
            width: miniSidenav ? 96 : 320,
            marginBottom:0,
            left: 0,
            transform: 'translateX(0)',
          },
          [theme.breakpoints.down('xl')]: {
            transform: miniSidenav ? 'translateX(-320px)' : 'translateX(0)',
          },
        },
      })}
    >
      {children}
    </Drawer>
  );
};

export default SidenavDrawer;