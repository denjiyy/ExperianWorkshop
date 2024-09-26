import Icon from "@mui/material/Icon";
import profilePicture from "../../images/team-4.jpg";
import { Avatar } from "@mui/material";
import { LoginForm } from "../pages/Login/elements";
const routes = [
  {
    type: "nocollapse",
    name: "Dashboards",
    key: "dashboards",
    route: "/dashboards", // Add the route for navigation
    component: <LoginForm />,
  },
  {
    type: "title",
    title: "Pages",
    key: "title-pages",
  },
  {
    type: "nocollapse",
    name: "Sales",
    key: "sales",
    route: "/dashboards/sales",
    component: <LoginForm />, // In a real app, you might load the Sales component
  },
  {
    type: "divider",
    key: "divider-0",
  },
  {
    type: "profile",
    name: "Brooklyn Alice",
    key: "brooklyn-alice",
    icon: (
      <Avatar
        src={profilePicture}
        alt="Brooklyn Alice"
        sx={{ width: 24, height: 24 }}
      />
    ),
    component: <LoginForm />,
  },
];

export default routes;
