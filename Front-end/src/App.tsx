import { Route, Routes } from "react-router";
import "./App.css";
import PostPage from "./pages/PostPage";
import Login from "./pages/Login";
import Register from "./pages/Register";
import { Navigate } from "react-router-dom";

function App() {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/post" />} />
      <Route path="/post/*" element={<PostPage />} />
      <Route path="login" element={<Login />} />
      <Route path="Register" element={<Register />} />
    </Routes>
  );
}

export default App;
