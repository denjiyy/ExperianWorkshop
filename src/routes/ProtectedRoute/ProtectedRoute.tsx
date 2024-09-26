import {Routes,Route,Link,Navigate,Outlet} from "react-router-dom"

export const ProtectedRoute = ({
    isAllowed,
    redirectedPath,
    children,
}:any)=>{
    if(!isAllowed){
        return <Navigate to={redirectedPath} replace/>
    }
    return children? children : <Outlet/>;
};