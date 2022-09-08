import React from "react";
import { Route, Routes } from "react-router-dom";
import { AuthRoute } from "../../components/auth/AuthRoute";
import { AuthRoutes } from "../../models/enums/AuthRoutes";
import LoginPage from "./LoginPage";
import ProfilePage from "./ProfilePage";
import SignupPage from "./SignupPage";

export default function User(): JSX.Element {
  return (
    <>
      <Routes>
        <Route path="signup" element={<SignupPage />} />
        <Route path="login" element={<LoginPage />} />
      </Routes>
      <AuthRoute path={AuthRoutes.profile} Component={ProfilePage} />
    </>
  );
}
