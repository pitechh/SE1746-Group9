import React, { useEffect, useState } from "react";
import { getRequest } from "../../services/apiHelper";
import Swal from "sweetalert2";
import { useNavigate } from "react-router-dom";
import { postRequest } from "../../services/apiHelper";

const ConferenceHostingRegistrationListByDelegate = ({
  delegateId,
  delegateName,
}) => {
  const [registrations, setRegistrations] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    fetchRegistrations();
  }, []);

  const fetchRegistrations = async () => {
    try {
      const response = await getRequest(
        `/ConferenceHostingRegistration/get-by-delegateId?id=${delegateId}`
      );
      if (response.isSuccess) {
        setRegistrations(response.data);
      } else {
        Swal.fire("Lỗi!", response.message, "error");
      }
    } catch (error) {
      Swal.fire("Lỗi!", "Không thể tải danh sách đơn đăng ký", "error");
    } finally {
      setLoading(false);
    }
  };
  const handleApproval = async (id, hostById, action) => {
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
          fetchRegistrations();
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
          fetchRegistrations();
        } else {
          Swal.fire("Lỗi!", response.message, "error");
        }
      } catch (error) {
        Swal.fire("Lỗi!", "Không thể xóa đơn", "error");
      }
    }
  };

  return (
    <div className="container mx-auto p-6">
      <h2 className="text-2xl font-bold mb-4 text-center">
        Danh sách các đơn đăng kí tổ chức hội thảo của {delegateName}
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
                <th className="py-3 px-6 text-left">ID</th>
                <th className="py-3 px-6 text-left">Tên Hội Thảo</th>
                <th className="py-3 px-6 text-left">Địa điểm</th>
                <th className="py-3 px-6 text-left">Ngày bắt đầu</th>
                <th className="py-3 px-6 text-left">Ngày kết thúc</th>
                <th className="py-3 px-6 text-left">Ngày Đăng Ký</th>
                <th className="py-3 px-6 text-left">Trạng Thái</th>
                <th className="py-3 px-6 text-left">Hành động</th>
              </tr>
            </thead>
            <tbody>
              {registrations.length > 0 ? (
                registrations.map((reg) => (
                  <tr
                    key={reg.id}
                    className="border-b hover:bg-gray-100 transition-colors"
                  >
                    <td className="py-2 px-4">{reg.id}</td>
                    <td className="py-2 px-4">{reg.name}</td>
                    <td className="py-2 px-4">{reg.location}</td>
                    <td className="py-2 px-4">
                      {reg.startDate
                        ? new Date(reg.startDate).toLocaleString()
                        : "Chưa đăng ký"}
                    </td>
                    <td className="py-2 px-4">
                      {reg.endDate
                        ? new Date(reg.endDate).toLocaleString()
                        : "Chưa đăng ký"}
                    </td>
                    <td className="py-2 px-4">
                      {reg.createAt
                        ? new Date(reg.createAt).toLocaleString()
                        : "Chưa đăng ký"}
                    </td>
                    <td className="py-2 px-4">
                      {reg.status ? reg.status : "Pending"}
                    </td>
                    <td className="py-4 px-6 flex space-x-2">
                      {reg.status === "Pending" ? (
                        <>
                          <button
                            onClick={() =>
                              handleApproval(reg.id, reg.registerId, "approve")
                            }
                            className="px-4 py-2 bg-green-600 text-white rounded"
                          >
                            Duyệt
                          </button>
                          <button
                            onClick={() => handleApproval(reg.id, "deny")}
                            className="px-4 py-2 bg-red-600 text-white rounded"
                          >
                            Từ chối
                          </button>
                        </>
                      ) : (
                        <button
                          onClick={() => handleDelete(reg.id)}
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
                  <td colSpan="4" className="text-center p-4">
                    Không có đơn đăng ký nào.
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

export default ConferenceHostingRegistrationListByDelegate;
