import { Navigate, Outlet } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

const PrivateRoute = () => {
  const token = localStorage.getItem("token");

  if (!token) {
    return <Navigate to="/login" />; // Chuyển hướng nếu chưa đăng nhập
  }

  try {
    const decoded = jwtDecode(token);
    if (decoded.isAdmin !== "True") {
      return <Navigate to="/forbiden" />; // Nếu không phải Admin, chuyển hướng về trang chủ
    }
  } catch (error) {
    return <Navigate to="/login" />; // Nếu token không hợp lệ, chuyển hướng về login
  }

  return <Outlet />; // Hiển thị nội dung của route nếu đủ quyền
};

export default PrivateRoute;
