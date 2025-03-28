import { Link } from "react-router-dom";

const Forbidden = () => {
  return (
    <div className="flex flex-col items-center justify-center h-screen bg-gray-100">
      <h1 className="text-4xl font-bold text-red-600 mb-4">403 Forbidden</h1>
      <p className="text-lg text-gray-700 mb-6">
        Bạn không có quyền truy cập vào trang này.
      </p>
      <Link to="/" className="px-4 py-2 bg-blue-500 text-white rounded">
        Quay lại trang chủ
      </Link>
    </div>
  );
};

export default Forbidden;
