import { Navigate, Route, Routes } from "react-router-dom";
import Cookies from "js-cookie";
import { NonAuthRoutes } from "../../models/enums/AuthRoutes";

interface Props {
  Component: any;
  path: string;
}

export const AuthRoute = ({ Component, path }: Props): JSX.Element => {
  const isAuthed = Cookies.get();
  console.log(isAuthed);

  const message = "لطفا برای مشاهده این صفحه ابتدا وارد شوید";
  return (
    <Routes>
      <Route
        path={path}
        element={
          isAuthed ? (
            <Component />
          ) : (
            <Navigate
              to={NonAuthRoutes.login}
              state={{ requestedPath: path, message: message }}
            />
          )
        }
      />
    </Routes>
  );
};
