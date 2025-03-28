import React, { useEffect, useState } from "react";
import { getRequest } from "../../services/apiHelper";

const Delegates = () => {
  const [delegates, setDelegates] = useState([]);
  const [searchEmail, setSearchEmail] = useState("");
  const [selectedDelegate, setSelectedDelegate] = useState(null);

  useEffect(() => {
    fetchDelegates();
  }, []);

  const fetchDelegates = async (email = "") => {
    try {
      const url = email
        ? `/Delegates/get-by-email?email=${email}`
        : "/Delegates/get-all";
      const response = await getRequest(url);
      setDelegates(response.data || []);
    } catch (error) {
      alert("Lỗi khi tải danh sách delegates!");
    }
  };

  const handleSearchChange = (e) => {
    setSearchEmail(e.target.value);
    fetchDelegates(e.target.value);
  };

  const handleViewDetails = (delegate) => {
    setSelectedDelegate(delegate);
  };

  const closeModal = () => {
    setSelectedDelegate(null);
  };

  return (
    <div className="container mx-auto p-6">
      <h2 className="text-2xl font-bold mb-4">Danh Sách Đại biểu</h2>
      <input
        type="text"
        value={searchEmail}
        onChange={handleSearchChange}
        placeholder="Tìm kiếm theo Email..."
        className="border p-2 rounded w-full mb-4"
      />
      <table className="w-full border-collapse border border-gray-300">
        <thead>
          <tr className="bg-gray-200">
            <th className="border p-2">ID</th>
            <th className="border p-2">Họ Tên</th>
            <th className="border p-2">Email</th>
            <th className="border p-2">SĐT</th>
            <th className="border p-2">Tổ chức</th>
            <th className="border p-2">Chức vụ</th>
            <th className="border p-2">Hành động</th>
          </tr>
        </thead>
        <tbody>
          {delegates.length > 0 ? (
            delegates.map((delegate) => (
              <tr key={delegate.id} className="border">
                <td className="border p-2">{delegate.id}</td>
                <td className="border p-2">{delegate.fullName}</td>
                <td className="border p-2">{delegate.email}</td>
                <td className="border p-2">{delegate.phone}</td>
                <td className="border p-2">{delegate.organization}</td>
                <td className="border p-2">{delegate.position}</td>
                <td className="border p-2">
                  <button
                    onClick={() => handleViewDetails(delegate)}
                    className="bg-blue-500 text-white px-4 py-2 rounded"
                  >
                    Xem chi tiết
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan="7" className="text-center p-4">
                Không có đại biểu nào.
              </td>
            </tr>
          )}
        </tbody>
      </table>

      {selectedDelegate && (
        <div className="fixed inset-0 flex items-center justify-center bg-gray-900 bg-opacity-50">
          <div className="bg-white p-6 rounded-lg shadow-lg w-96">
            <h2 className="text-xl font-bold mb-4">Thông tin Đại biểu</h2>
            <p>
              <strong>Họ Tên:</strong> {selectedDelegate.fullName}
            </p>
            <p>
              <strong>Email:</strong> {selectedDelegate.email}
            </p>
            <p>
              <strong>Số điện thoại:</strong> {selectedDelegate.phone}
            </p>
            <p>
              <strong>Tổ chức:</strong> {selectedDelegate.organization}
            </p>
            <p>
              <strong>Chức vụ:</strong> {selectedDelegate.position}
            </p>
            <p>
              <strong>Giới tính:</strong> {selectedDelegate.gender}
            </p>
            <p>
              <strong>Ngày sinh:</strong> {selectedDelegate.dateOfBirth}
            </p>
            <p>
              <strong>Quốc tịch:</strong> {selectedDelegate.nationality}
            </p>
            <p>
              <strong>Địa chỉ:</strong> {selectedDelegate.address}
            </p>
            <p>
              <strong>Tiểu sử:</strong> {selectedDelegate.biography}
            </p>
            <button
              onClick={closeModal}
              className="mt-4 bg-red-500 text-white px-4 py-2 rounded w-full"
            >
              Đóng
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default Delegates;
