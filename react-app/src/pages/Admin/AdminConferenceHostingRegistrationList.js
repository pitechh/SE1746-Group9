import React, { useEffect, useState } from "react";
import { getRequest, postRequest } from "../../services/apiHelper";
import Swal from "sweetalert2";
import { useNavigate } from "react-router-dom";

const AdminConferenceHostingRegistrationList = () => {
  const [conferences, setConferences] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetchConferences();
  }, []);

  const fetchConferences = async () => {
    try {
      const response = await getRequest(
        "/ConferenceHostingRegistration/get-all"
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

  // Gọi API tạo hội thảo sau khi duyệt
  const createConference = async (conferenceData) => {
    try {
      const response = await postRequest("/Conferences/create", conferenceData);
      if (response.isSuccess) {
        Swal.fire("Thành công!", "Hội thảo đã được tạo", "success");
      } else {
        Swal.fire("Lỗi!", response.message, "error");
      }
    } catch (error) {
      Swal.fire("Lỗi!", "Không thể tạo hội thảo", "error");
    }
  };

  const handleApproval = async (id, action, conferenceData) => {
    const confirm = await Swal.fire({
      title: `Bạn có chắc chắn muốn ${
        action === "approve" ? "duyệt" : "từ chối"
      } đơn này?`,
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Có",
      cancelButtonText: "Không",
    });

    if (confirm.isConfirmed) {
      try {
        const response = await postRequest(
          `/ConferenceHostingRegistration/${action}?id=${id}`
        );
        if (response.isSuccess) {
          Swal.fire("Thành công!", response.message, "success");

          // Nếu duyệt thành công thì tạo hội thảo
          if (action === "approve") {
            await createConference(conferenceData);
          }
          fetchConferences();
        } else {
          Swal.fire("Lỗi!", response.message, "error");
        }
      } catch (error) {
        Swal.fire("Lỗi!", "Không thể cập nhật trạng thái", "error");
      }
    }
  };

  const handleDelete = async (id) => {
    const confirm = await Swal.fire({
      title: "Bạn có chắc chắn muốn xóa đơn này?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
      confirmButtonText: "Xóa",
      cancelButtonText: "Hủy",
    });

    if (confirm.isConfirmed) {
      try {
        const response = await postRequest(
          `/ConferenceHostingRegistration/delete?id=${id}`
        );
        if (response.isSuccess) {
          Swal.fire("Thành công!", "Đơn đã được xóa", "success");
          fetchConferences();
        } else {
          Swal.fire("Lỗi!", response.message, "error");
        }
      } catch (error) {
        Swal.fire("Lỗi!", "Không thể xóa đơn", "error");
      }
    }
  };

  return (
    <div className="p-6 bg-gray-100 min-h-screen">
      <h2 className="text-2xl font-semibold text-center mb-6 text-gray-700">
        Danh Sách Đăng Ký Tổ Chức Hội Thảo
      </h2>

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
                <th className="py-3 px-6 text-left">Người đăng ký</th>
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
                    <td
                      className="py-4 px-6 text-blue-600 cursor-pointer hover:underline"
                      onClick={() =>
                        navigate(`/admin/delegate-details/${conf.registerId}`, {
                          state: { registerName: conf.registerName },
                        })
                      }
                    >
                      {conf.registerName}
                    </td>
                    <td className="py-4 px-6 flex space-x-2">
                      {conf.status === "Pending" ? (
                        <>
                          <button
                            onClick={() =>
                              handleApproval(conf.id, "approve", {
                                id: "",
                                name: conf.name,
                                description: conf.description,
                                startDate: conf.startDate,
                                endDate: conf.endDate,
                                location: conf.location,
                                hostBy: conf.registerId,
                              })
                            }
                            className="px-4 py-2 bg-green-600 text-white rounded"
                          >
                            Duyệt
                          </button>
                          <button
                            onClick={() => handleApproval(conf.id, "deny")}
                            className="px-4 py-2 bg-red-600 text-white rounded"
                          >
                            Từ chối
                          </button>
                        </>
                      ) : (
                        <button
                          onClick={() => handleDelete(conf.id)}
                          className="px-4 py-2 bg-gray-600 text-white rounded"
                        >
                          Xóa đơn
                        </button>
                      )}
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="7" className="text-center py-4 text-gray-500">
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

export default AdminConferenceHostingRegistrationList;
