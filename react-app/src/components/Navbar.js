import { Link, useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

const Navbar = () => {
  const token = localStorage.getItem("token"); // Kiểm tra token
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  let isAdmin = false;

  if (token) {
    try {
      const decoded = jwtDecode(token);
      isAdmin = decoded.isAdmin === "True"; // Kiểm tra quyền Admin
    } catch (error) {
      isAdmin = false;
    }
  }

  return (
    <nav className="bg-blue-500 p-4 text-white flex justify-between">
      <div className="text-xl font-bold">
        <Link to="/">Conference Portal</Link>
      </div>
      <div className="space-x-4">
        <Link to="/" className="hover:underline">
          Home
        </Link>
        {token ? (
          <>
            {/* Nếu user là Admin, hiển thị các mục quản lý */}
            {isAdmin && (
              <>
                <Link to="/admin/delegates">Manage Delegates</Link>
                <Link to="/admin/conferences">Manage Conferences</Link>
              </>
            )}

            {!isAdmin && (
              <>
                <Link to="/delegates">Delegates</Link>
                <Link to="/conferences">Conferences</Link>
              </>
            )}
            <Link to="/profile" className="hover:underline">
              Profile
            </Link>
            <button
              onClick={handleLogout}
              className="ml-4 bg-red-500 px-3 py-1 rounded"
            >
              Logout
            </button>
          </>
        ) : (
          <Link to="/login" className="hover:underline">
            Login
          </Link>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
