import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getRequest, postRequest } from "../../services/apiHelper";
import Swal from "sweetalert2"; // Import SweetAlert2

const ConferenceDetails = () => {
  const { id } = useParams();
  const [conference, setConference] = useState(null);
  const [registrations, setRegistrations] = useState([]);
  const [roles, setRoles] = useState([]);
  const [selectedRole, setSelectedRole] = useState("");
  const [registrationStatus, setRegistrationStatus] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    fetchConferenceDetails();
    fetchRegistrations();
    fetchConferenceRoles();
    checkRegistrationStatus();
  }, [id]);

  const checkRegistrationStatus = async () => {
    try {
      const response = await postRequest(
        `/registrations/delegates-check-registration-status?conferenceId=${id}`
      );
      setRegistrationStatus(response.isSuccess ? response.data : null);
    } catch (error) {
      console.error("Lỗi khi kiểm tra trạng thái đăng ký:", error);
    }
  };

  const fetchConferenceDetails = async () => {
    try {
      const response = await getRequest(`/conferences/get-by-id?id=${id}`);
      if (response.data.hostByMe === true) {
        navigate(`/hosting/conference/${response.data.id}`);
      }
      setConference(response.data);
    } catch (error) {
      console.error("Lỗi khi tải thông tin hội thảo:", error);
    }
  };

  const fetchRegistrations = async () => {
    try {
      const response = await getRequest(
        `/registrations/get-by-id?conferenceId=${id}`
      );
      setRegistrations(response.data || []);
    } catch (error) {
      console.error("Lỗi khi tải danh sách đăng ký:", error);
    }
  };

  const fetchConferenceRoles = async () => {
    try {
      const response = await getRequest("/conferenceRoles/get-all");
      if (response && response.data) setRoles(response.data);
    } catch (error) {
      console.error("Lỗi khi tải danh sách vai trò hội thảo:", error);
    }
  };

  // Xử lý đăng ký tham gia hội thảo
  const handleRegister = async () => {
    if (!selectedRole) {
      Swal.fire("Lỗi", "Vui lòng chọn vai trò trước khi đăng ký!", "warning");
      return;
    }

    const requestData = {
      ConferenceId: id,
      ConferenceRoleId: selectedRole,
      Status: "Pending",
    };

    try {
      const response = await postRequest(
        "/registrations/delegate-register-conference",
        requestData
      );

      if (response.isSuccess) {
        Swal.fire(
          "Thành công!",
          "Bạn đã đăng ký tham gia hội thảo!",
          "success"
        );
        fetchRegistrations();
        checkRegistrationStatus();
      } else {
        Swal.fire("Lỗi", `Đăng ký thất bại: ${response.Message}`, "error");
      }
    } catch (error) {
      Swal.fire("Lỗi", "Đã xảy ra lỗi khi đăng ký", "error");
    }
  };

  const handleUnregister = async () => {
    Swal.fire({
      title: "Bạn có chắc chắn muốn hủy đăng ký?",
      text: "Hành động này không thể hoàn tác!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
      confirmButtonText: "Xác nhận",
      cancelButtonText: "Hủy",
    }).then(async (result) => {
      if (result.isConfirmed) {
        try {
          const response = await postRequest(
            `/registrations/unregistered-registration?conferenceId=${id}`
          );

          if (response.isSuccess) {
            Swal.fire(
              "Thành công!",
              response.message || "Hủy đăng ký thành công!",
              "success"
            );
            fetchRegistrations();
            checkRegistrationStatus();
          } else {
            Swal.fire("Lỗi", "Hủy đăng ký thất bại!", "error");
          }
        } catch (error) {
          Swal.fire("Lỗi", "Đã xảy ra lỗi khi hủy đăng ký", "error");
        }
      }
    });
  };

  if (!conference)
    return (
      <div className="flex justify-center items-center h-screen">
        <p className="text-gray-500 text-lg">Đang tải...</p>
      </div>
    );

  return (
    <div>
      <div className="mx-auto mt-10 p-6 bg-white shadow-lg rounded-xl">
        <h2 className="text-2xl font-semibold text-gray-800 mb-4 border-b pb-2">
          Chi Tiết Hội Thảo
        </h2>
        <div className="space-y-4">
          {conference.hostByMe === true && (
            <h3 className="text-xl font-semibold text-gray-700 mb-3">
              Bạn là người tổ chức hội thảo này
            </h3>
          )}
          <p>
            <strong className="text-gray-700">Tên:</strong> {conference.name}
          </p>
          <p>
            <strong className="text-gray-700">Mô Tả:</strong>{" "}
            {conference.description}
          </p>
          <p>
            <strong className="text-gray-700">Ngày Bắt Đầu:</strong>{" "}
            {new Date(conference.startDate).toLocaleString()}
          </p>
          <p>
            <strong className="text-gray-700">Ngày Kết Thúc:</strong>{" "}
            {new Date(conference.endDate).toLocaleString()}
          </p>
          <p>
            <strong className="text-gray-700">Địa Điểm:</strong>{" "}
            {conference.location}
          </p>
          <p>
            <strong className="text-gray-700">Tổ chức bởi:</strong>{" "}
            <span className="text-blue-700 text-underline hover:pointer">
              {conference.hostByName}
            </span>
          </p>
        </div>
        <button
          className="px-4 py-2 bg-blue-600 text-white rounded-lg shadow-md hover:bg-blue-700 transition mt-4"
          onClick={() => navigate(-1)}
        >
          Quay Lại
        </button>
      </div>
      {conference.hostByMe === false && (
        <div className="mt-6 p-6 bg-white shadow-lg rounded-xl">
          <h3 className="text-xl font-semibold text-gray-700 mb-3">
            Đăng ký tham gia hội thảo
          </h3>
          <div className="flex items-center space-x-4">
            {registrationStatus === "Not registered" && (
              <>
                <select
                  className="border p-2 rounded mr-5"
                  value={selectedRole}
                  onChange={(e) => setSelectedRole(e.target.value)}
                >
                  <option value="">Chọn vai trò</option>
                  {roles.map((role) => (
                    <option key={role.id} value={role.id}>
                      {role.name}
                    </option>
                  ))}
                </select>
                <button
                  className="px-4 py-2 bg-green-600 text-white rounded-lg shadow-md hover:bg-green-700 transition"
                  onClick={handleRegister}
                >
                  Đăng ký ngay
                </button>
              </>
            )}
            {registrationStatus === "Cancelled" && (
              <>
                <p>Bạn đã bị tổ chức từ chối đơn đăng ký</p>

                <button
                  className="px-4 py-2 bg-green-600 text-white rounded-lg shadow-md hover:bg-green-700 transition"
                  onClick={handleUnregister}
                >
                  Hủy đăng ký và đăng ký lại
                </button>
              </>
            )}
            {registrationStatus === "Pending" && (
              <>
                <p className="my-5">
                  Bạn đã đăng ký, chờ Ban quản trị xét duyệt.
                </p>
                <button
                  className="px-4 py-2 bg-red-600 text-white rounded-lg shadow-md hover:bg-red-700 transition"
                  onClick={handleUnregister}
                >
                  Hủy đăng ký
                </button>
              </>
            )}

            {registrationStatus === "Confirmed" && (
              <>
                <p className="my-5">Bạn đã được xét duyệt tham gia.</p>
                <button
                  className="px-4 py-2 bg-red-600 text-white rounded-lg shadow-md hover:bg-red-700 transition"
                  onClick={handleUnregister}
                >
                  Hủy đăng ký
                </button>
              </>
            )}
          </div>
        </div>
      )}

      {/* Danh sách đại biểu đã đăng ký */}
      <div className="mt-6">
        <h3 className="text-xl font-semibold text-gray-700 mb-3">
          Danh Sách Đại Biểu Tham Gia
        </h3>
        {registrations.length > 0 ? (
          <table className="w-full border-collapse border border-gray-300 text-sm">
            <thead>
              <tr className="bg-gray-200">
                <th className="border border-gray-300 px-4 py-2">STT</th>
                <th className="border border-gray-300 px-4 py-2">
                  Tên Đại Biểu
                </th>
                <th className="border border-gray-300 px-4 py-2">Email</th>
                <th className="border border-gray-300 px-4 py-2">
                  Ngày Đăng Ký
                </th>
                <th className="border border-gray-300 px-4 py-2">Trạng Thái</th>
              </tr>
            </thead>
            <tbody>
              {registrations.map((delegate, index) => (
                <tr key={delegate.id} className="text-center">
                  <td className="border border-gray-300 px-4 py-2">
                    {index + 1}
                  </td>
                  <td className="border border-gray-300 px-4 py-2">
                    {delegate.delegateName}
                  </td>
                  <td className="border border-gray-300 px-4 py-2">
                    {delegate.delegateEmail}
                  </td>
                  <td className="border border-gray-300 px-4 py-2">
                    {delegate.registeredAt
                      ? new Date(delegate.registeredAt).toLocaleString()
                      : "Chưa đăng ký"}
                  </td>
                  <td className="border border-gray-300 px-4 py-2">
                    {delegate.status}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <p className="text-gray-500">Chưa có đại biểu nào đăng ký.</p>
        )}
      </div>
    </div>
  );
};

export default ConferenceDetails;
