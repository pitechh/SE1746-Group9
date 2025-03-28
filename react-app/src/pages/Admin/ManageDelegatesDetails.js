import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { postRequest } from "../../services/apiHelper";
import Swal from "sweetalert2";
import ConferenceHostingRegistrationListByDelegate from "./ConferenceHostingRegistrationByDelegateList";

const ManageDelegatesDetails = () => {
  const { id } = useParams();
  const [delegate, setDelegate] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetchDelegateDetails();
  }, [id]);

  const fetchDelegateDetails = async () => {
    try {
      const response = await postRequest(`/Delegates/get-by-id?id=${id}`);
      if (response.isSuccess) {
        setDelegate(response.data);
      } else {
        Swal.fire("Lỗi!", response.message, "error");
      }
    } catch (error) {
      Swal.fire("Lỗi!", "Không thể lấy thông tin đại biểu", "error");
    }
  };

  if (!delegate) {
    return (
      <div className="flex justify-center items-center h-screen">
        <p className="text-gray-500">Đang tải...</p>
      </div>
    );
  }

  return (
    <div>
      <div className="mx-auto p-6 bg-white shadow-lg rounded-lg mt-10">
        <h2 className="text-2xl font-bold mb-4">Chi tiết đại biểu</h2>
        <div className="space-y-4 text-gray-700">
          <p>
            <strong>ID:</strong> {delegate.id}
          </p>
          <p>
            <strong>Họ tên:</strong> {delegate.fullName}
          </p>
          <p>
            <strong>Email:</strong> {delegate.email}
          </p>
          <p>
            <strong>Số điện thoại:</strong> {delegate.phone}
          </p>
          <p>
            <strong>Tổ chức:</strong> {delegate.organization || "Không có"}
          </p>
          <p>
            <strong>Chức vụ:</strong> {delegate.position || "Không có"}
          </p>
          <p>
            <strong>Địa chỉ:</strong> {delegate.address || "Không có"}
          </p>
          <p>
            <strong>Giới tính:</strong> {delegate.gender}
          </p>
          <p>
            <strong>Ngày tham gia:</strong> {delegate.createdAt}
          </p>
          <p>
            <strong>Ngày sinh:</strong> {delegate.dateOfBirth || "Không có"}
          </p>
        </div>
        <button
          onClick={() => navigate(-1)}
          className="mt-6 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
        >
          Quay lại
        </button>
      </div>
      <div className="mx-auto p-6 bg-white shadow-lg rounded-lg mt-10">
        <ConferenceHostingRegistrationListByDelegate
          delegateId={delegate.id}
          delegateName={delegate.fullName}
        />
      </div>
    </div>
  );
};

export default ManageDelegatesDetails;
