import React from "react";
import clsx from "clsx";
import { Box, makeStyles } from "@mui/material";

import {
  Drawer,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Divider,
} from "@mui/material";
// import List from "@material-ui/core/List";
// import ListItem from "@material-ui/core/ListItem";
// import ListItemIcon from "@material-ui/core/ListItemIcon";
// import ListItemText from "@material-ui/core/ListItemText";
// import InBoxIcon from "@material-ui/icons/MoveToInbox";
// import HomeIcon from "@material-ui/icons/Home";
// import MailIcon from "@material-ui/icons/Mail";
// import Divider from "@material-ui/core/Divider";
import HomeIcon from '@mui/icons-material/Home';

type Anchor = "top" | "left" | "bottom" | "right";

const LeftNavDrawer = React.forwardRef((props, ref) => {
  const [state, setState] = React.useState({
    top: false,
    left: false,
    bottom: false,
    right: false,
  });

  const toggleDrawer =
    (anchor: Anchor, open: boolean) =>
    (event: React.KeyboardEvent | React.MouseEvent) => {
      if (
        event.type === "keydown" &&
        ((event as React.KeyboardEvent).key === "Tab" ||
          (event as React.KeyboardEvent).key === "Shift")
      ) {
        return;
      }
      setState({ ...state, [anchor]: open });
    };
  // The useImperativeHandle() React method contains the method being called from the TopAppBar components button onClick. Here I am just changing the state to open the nav drawer.
  React.useImperativeHandle(ref, () => ({
    toggleFromParent(anchor: Anchor) {
      setState({ ...state, [anchor]: true });
    },
  }));

  const list = (anchor: Anchor) => (
    <Box
    sx={{
      width: anchor === "top" || anchor === "bottom" ? "auto" : 250,
    }}
  >
      <List>
        {["Inbox", "Starred", "Send email", "Drafts"].map((text, index) => (
          <ListItem component="button" key={text}>
            <ListItemIcon>
              {index % 2 === 0 ? <HomeIcon /> : <HomeIcon />}
            </ListItemIcon>
            <ListItemText primary={text} />
          </ListItem>
        ))}
      </List>
      <Divider />
      <List>
        <ListItem component="button" key={"Home"}>
          <ListItemIcon>
            <HomeIcon></HomeIcon>
          </ListItemIcon>
          <ListItemText primary={"Home"} />
        </ListItem>
      </List>
    </Box>
  );

  return (
    <div>
      {(["left", "right", "top", "bottom"] as Anchor[]).map((anchor) => (
        <React.Fragment key={anchor}>
          <Drawer
            anchor={anchor}
            open={state[anchor]}
            onClose={toggleDrawer(anchor, false)}
          >
            {list(anchor)}
          </Drawer>
        </React.Fragment>
      ))}
    </div>
  );
});

export { LeftNavDrawer as default };
