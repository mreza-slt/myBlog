import { Route, Routes } from "react-router";
import "./App.css";
import Login from "./pages/Login";

function App() {
  return (
    <div>
      <Routes>
        <Route path="login" element={<Login />} />
      </Routes>
    </div>
  );
}

export default App;
