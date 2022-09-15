import React from "react";
import { Route, Routes } from "react-router-dom";
import Logout from "../../components/user/Logout";
import LoginPage from "./LoginPage";
import ProfilePage from "./ProfilePage";
import SignupPage from "./SignupPage";

export default function User(): JSX.Element {
  return (
    <Routes>
      <Route path="signup" element={<SignupPage />} />
      <Route path="login" element={<LoginPage />} />
      <Route path="logout" element={<Logout />} />
      <Route path="profile" element={<ProfilePage />} />
    </Routes>
  );
}
