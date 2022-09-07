import { Route, Routes } from "react-router";
import "./App.css";
import PostPage from "./pages/PostPage";
import { Navigate } from "react-router-dom";
import Layout from "./layout/Layout";
import User from "./pages/user/User";

function App() {
  return (
    <Layout>
      <Routes>
        <Route path="/" element={<Navigate to="/post" />} />
        <Route path="/post/*" element={<PostPage />} />
        <Route path="/user/*" element={<User />} />
      </Routes>
    </Layout>
  );
}

export default App;
