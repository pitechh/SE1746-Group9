import React, { useEffect, useState } from "react";
import { getRequest, postRequest } from "../../services/apiHelper";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";

const ConferenceHostingRegistrationList = () => {
  const [conferences, setConferences] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetchConferences();
  }, []);

  const fetchConferences = async () => {
    try {
      const response = await getRequest(
        "/ConferenceHostingRegistration/get-my-registrations"
      );
      if (response.isSuccess) {
        setConferences(response.data);
      } else {
        Swal.fire("Lỗi!", response.message, "error");
      }
    } catch (error) {
      Swal.fire("Lỗi!", "Không thể lấy danh sách hội thảo", "error");
    } finally {
      setLoading(false);
    }
  };

  const handleCancelRegistration = async (id) => {
    const confirm = await Swal.fire({
      title: "Bạn có chắc chắn muốn hủy?",
      text: "Bạn sẽ không thể khôi phục đăng ký này!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
      confirmButtonText: "Có, hủy ngay!",
      cancelButtonText: "Không",
    });

    if (confirm.isConfirmed) {
      try {
        const response = await postRequest(
          `/ConferenceHostingRegistration/delegate-delete?id=${id}`
        );
        if (response.isSuccess) {
          Swal.fire("Đã hủy!", "Đăng ký của bạn đã được hủy.", "success");
          fetchConferences();
        } else {
          Swal.fire("Lỗi!", response.message, "error");
        }
      } catch (error) {
        Swal.fire("Lỗi!", "Không thể hủy đăng ký", "error");
      }
    }
  };

  return (
    <div className="p-6 bg-gray-100 min-h-screen">
      <h2 className="text-2xl font-semibold text-center mb-6 text-gray-700">
        Danh Sách Hội Thảo Đã Đăng Ký
      </h2>

      <div className="flex justify-end mb-4">
        <button
          onClick={() => navigate("/conference-hosting-registration-form")}
          className="px-4 py-2 bg-blue-600 text-white rounded"
        >
          Đăng ký hội thảo
        </button>
      </div>

      {loading ? (
        <div className="flex justify-center">
          <div className="animate-spin rounded-full h-10 w-10 border-b-2 border-blue-500"></div>
        </div>
      ) : (
        <div className="overflow-x-auto">
          <table className="min-w-full bg-white shadow-md rounded-lg overflow-hidden">
            <thead className="bg-blue-500 text-white">
              <tr>
                <th className="py-3 px-6 text-left">Tên hội thảo</th>
                <th className="py-3 px-6 text-left">Ngày bắt đầu</th>
                <th className="py-3 px-6 text-left">Ngày kết thúc</th>
                <th className="py-3 px-6 text-left">Địa điểm</th>
                <th className="py-3 px-6 text-left">Trạng thái</th>
                <th className="py-3 px-6 text-left">Hành động</th>
              </tr>
            </thead>
            <tbody>
              {conferences.length > 0 ? (
                conferences.map((conf, index) => (
                  <tr
                    key={conf.id}
                    className={`border-b ${
                      index % 2 === 0 ? "bg-gray-50" : "bg-white"
                    } hover:bg-blue-100 transition-all`}
                  >
                    <td className="py-4 px-6">{conf.name}</td>
                    <td className="py-4 px-6">
                      {new Date(conf.startDate).toLocaleDateString()}
                    </td>
                    <td className="py-4 px-6">
                      {new Date(conf.endDate).toLocaleDateString()}
                    </td>
                    <td className="py-4 px-6">{conf.location}</td>
                    <td
                      className={`py-4 px-6 font-semibold ${
                        conf.status === "Approved"
                          ? "text-green-600"
                          : conf.status === "Pending"
                          ? "text-yellow-600"
                          : "text-red-600"
                      }`}
                    >
                      {conf.status}
                    </td>
                    <td className="py-4 px-6">
                      <button
                        onClick={() => handleCancelRegistration(conf.id)}
                        className="px-4 py-2 bg-red-600 text-white rounded"
                      >
                        Hủy đăng ký
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="6" className="text-center py-4 text-gray-500">
                    Chưa có hội thảo nào được đăng ký.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default ConferenceHostingRegistrationList;
