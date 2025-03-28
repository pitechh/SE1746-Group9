import { useEffect, useState } from "react";
import { getRequest } from "../../services/apiHelper";
import { useNavigate } from "react-router-dom";

const Conferences = () => {
  const [conferences, setConferences] = useState([]);
  const [searchName, setSearchName] = useState("");
  const navigate = useNavigate(); // Hook điều hướng

  useEffect(() => {
    fetchConferences();
  }, []);

  const handleSearchChange = (e) => {
    setSearchName(e.target.value);
    fetchConferences(e.target.value);
  };

  const fetchConferences = async (name) => {
    try {
      const url = name
        ? `/Conferences/get-by-name?name=${name}`
        : "/Conferences/get-all";

      const response = await getRequest(url);
      setConferences(response.data || []);
    } catch (error) {
      alert("Lỗi khi tải danh sách hội thảo!");
    }
  };

  const handleNavigateToDetails = (id) => {
    navigate(`/conference/${id}`);
  };

  const handleNavigateToRegistrations = () => {
    navigate(`/conference-hosting-registration-list`); // Điều hướng đến trang danh sách đăng ký tổ chức hội thảo
  };

  return (
    <div className="container mx-auto p-6">
      <h2 className="text-2xl font-bold mb-4">Danh sách Hội Thảo</h2>

      {/* Thanh tìm kiếm */}
      <input
        type="text"
        value={searchName}
        onChange={handleSearchChange}
        placeholder="Tìm kiếm theo tên hội thảo..."
        className="border p-2 rounded w-full mb-4"
      />

      {/* Nút điều hướng đến danh sách đơn tổ chức hội thảo */}
      <div className="flex justify-end mb-4">
        <button
          onClick={handleNavigateToRegistrations}
          className="bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600 transition"
        >
          Xem các đơn đăng kí tổ chức hội thảo
        </button>
      </div>

      <table className="w-full border-collapse border border-gray-300 shadow-md rounded-lg overflow-hidden">
        <thead>
          <tr className="bg-gray-200 text-gray-700">
            <th className="border p-2">Tên Hội Thảo</th>
            <th className="border p-2">Ngày Bắt Đầu</th>
            <th className="border p-2">Ngày Kết Thúc</th>
            <th className="border p-2">Địa Điểm</th>
            <th className="border p-2">Thao Tác</th>
          </tr>
        </thead>
        <tbody>
          {conferences.length > 0 ? (
            conferences.map((conf) => (
              <tr key={conf.id} className="border hover:bg-blue-100 transition">
                <td className="border p-2">{conf.name}</td>
                <td className="border p-2">
                  {new Date(conf.startDate).toLocaleDateString()}
                </td>
                <td className="border p-2">
                  {new Date(conf.endDate).toLocaleDateString()}
                </td>
                <td className="border p-2">{conf.location}</td>
                <td className="border p-2 text-center">
                  <button
                    onClick={(e) => {
                      e.preventDefault();
                      handleNavigateToDetails(conf.id);
                    }}
                    className="bg-blue-500 text-white px-3 py-1 rounded hover:bg-blue-600 transition"
                  >
                    Xem chi tiết
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="5" className="text-center py-4 text-gray-500">
                Không có hội thảo nào.
              </td>
            </tr>
          )}
        </tbody>
      </table>
    </div>
  );
};

export default Conferences;
