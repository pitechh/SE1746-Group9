import React, { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getRequest, postRequest } from "../../services/apiHelper";
import Swal from "sweetalert2";
import { jwtDecode } from "jwt-decode";
import { Navigate } from "react-router-dom";

const ManageConferenceDetails = () => {
  const { id } = useParams();
  const [conference, setConference] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [editData, setEditData] = useState({});
  const [registrations, setRegistrations] = useState([]); // Danh sách đăng ký
  const [showPopup, setShowPopup] = useState(false);
  const [delegateEmail, setDelegateEmail] = useState("");
  const [delegateId, setDelegateId] = useState(null);
  const [conferenceRoles, setConferenceRoles] = useState([]);
  const [selectedRoleId, setSelectedRoleId] = useState("");
  const [selectedStatus, setSelectedStatus] = useState("Pending");
  const [selectedConferenceId, setSelectedConferenceId] = useState(null);
  const [searchDelegates, setSearchDelegates] = useState([]);

  const navigate = useNavigate();

  useEffect(() => {
    fetchConferenceDetails();
    fetchRegistrations();
  }, [id]);

  const token = localStorage.getItem("token");

  if (!token) {
    return <Navigate to="/login" />; // Chuyển hướng nếu chưa đăng nhập
  }

  const decoded = jwtDecode(token);
  console.log(decoded.isAdmin);

  const fetchConferenceDetails = async () => {
    try {
      const response = await getRequest(`/conferences/get-by-id?id=${id}`);
      if (response.data.hostByMe === true || decoded.isAdmin === "True") {
      } else {
        navigate("/forbiden");
      }
      setConference(response.data);
    } catch (error) {
      console.error("Error fetching conference details:", error);
    }
  };

  const handleSearchDelegate = async () => {
    try {
      const response = await getRequest(
        `/delegates/get-by-email?email=${delegateEmail}`
      );
      setSearchDelegates(response.data || []);

      if (response.data.length <= 0) {
        Swal.fire({
          title: "Không tìm thấy đại biểu",
          text: "Nhập đúng email và thử lại",
          icon: "warning",
          confirmButtonText: "OK",
        });
      }
    } catch (error) {
      Swal.fire({
        title: "Không tìm thấy đại biểu",
        text: error.message,
        icon: "warning",
        confirmButtonText: "OK",
      });
    }
  };

  const handleAddDelegateToConferences = async () => {
    if (!delegateId || !selectedRoleId || !selectedConferenceId) {
      Swal.fire({
        title: "",
        text: "Vui lòng chọn đầy đủ thông tin",
        icon: "warning",
        confirmButtonText: "OK",
      });
      return;
    }

    const registrationData = {
      ConferenceId: selectedConferenceId,
      ConferenceRoleId: selectedRoleId,
      Status: selectedStatus,
      DelegateId: delegateId,
    };
    let response;
    try {
      response = await postRequest(
        "/registrations/admin-add-delegates",
        registrationData
      );
      console.log(response.data);
      if (response.isSuccess) {
        Swal.fire({
          title: "",
          text: "Đăng kí tham gia thành công",
          icon: "success",
          confirmButtonText: "OK",
        });
        fetchRegistrations(selectedConferenceId); // Cập nhật danh sách đăng ký
        handleClosePopup();
      } else {
        Swal.fire({
          title: "Thất bại",
          text: response.message || "Đăng ký thất bại!",
          icon: "warning",
          confirmButtonText: "OK",
        });
      }
    } catch (error) {}
  };

  const fetchConferenceRoles = async () => {
    try {
      const response = await getRequest("/conferenceroles/get-all");
      setConferenceRoles(response.data || []);
    } catch (error) {
      //alert("Lỗi khi tải danh sách vai trò hội thảo!");
    }
  };

  const handleOpenPopup = (conferenceId) => {
    setSelectedConferenceId(conferenceId);
    setShowPopup(true);
    fetchConferenceRoles();
    fetchRegistrations(conferenceId);
  };

  const handleClosePopup = () => {
    setShowPopup(false);
    setDelegateEmail("");
    setDelegateId(null);
    setSelectedRoleId("");
    setSearchDelegates([]);
    setSelectedStatus("Pending");
  };

  // API lấy danh sách đại biểu đăng ký
  const fetchRegistrations = async () => {
    try {
      const response = await getRequest(
        `/registrations/get-by-id?conferenceId=${id}`
      );
      console.log(response.data);
      setRegistrations(response.data);
    } catch (error) {
      console.error("Error fetching registrations:", error);
    }
  };

  const handleUpdateStatus = async (registrationId, newStatus) => {
    const updateStatusData = {
      RegistrationId: registrationId,
      Status: newStatus,
    };
    try {
      const response = await postRequest(
        `/registrations/update-status?registrationId=${registrationId}`,
        updateStatusData
      );

      if (response.isSuccess) {
        Swal.fire({
          title: "Thành công",
          text: response.Message || "Cập nhật trạng thái thành công",
          icon: "success",
          confirmButtonText: "OK",
        });
        setRegistrations((prevRegistrations) =>
          prevRegistrations.map((regis) =>
            regis.id === registrationId
              ? { ...regis, status: newStatus }
              : regis
          )
        );
      } else {
        Swal.fire({
          title: "Thất bại",
          text: response.Message || "Cập nhật trạng thái thất bại",
          icon: "warning",
          confirmButtonText: "OK",
        });
      }
    } catch (error) {}
  };

  const handleEditClick = () => {
    setEditData(conference);
    setIsEditing(true);
  };

  const handleInputChange = (e) => {
    setEditData({ ...editData, [e.target.name]: e.target.value });
  };

  const handleSaveChanges = async () => {
    try {
      await postRequest(`/conferences/update?id=${id}`, editData);
      Swal.fire({
        title: "Thành công",
        text: "Cập nhật hội thảo thành công",
        icon: "success",
        confirmButtonText: "OK",
      });
      console.log("Dữ liệu trước khi gửi:", JSON.stringify(editData));
      setConference(editData);
      setIsEditing(false);
    } catch (error) {
      console.error("Error updating conference:", error);
    }
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
          <p>
            <strong className="text-gray-700">Tên:</strong>{" "}
            <span className="text-gray-900 font-medium">{conference.name}</span>
          </p>
          <p>
            <strong className="text-gray-700">Mô Tả:</strong>{" "}
            <span className="text-gray-900">{conference.description}</span>
          </p>
          <p>
            <strong className="text-gray-700">Ngày Bắt Đầu:</strong>{" "}
            <span className="text-gray-900">
              {new Date(conference.startDate).toLocaleString()}
            </span>
          </p>
          <p>
            <strong className="text-gray-700">Ngày Kết Thúc:</strong>{" "}
            <span className="text-gray-900">
              {new Date(conference.endDate).toLocaleString()}
            </span>
          </p>
          <p>
            <strong className="text-gray-700">Địa Điểm:</strong>{" "}
            <span className="text-gray-900">{conference.location}</span>
          </p>
          <p>
            <strong className="text-gray-700">Tổ chức bởi:</strong>{" "}
            <span
              className="text-blue-700 text-underline hover:pointer"
              onClick={() => {
                decoded.id === conference.hostById
                  ? navigate(`/profile`)
                  : navigate(`/admin/delegate-details/${conference.hostById}`);
              }}
            >
              {conference.hostByName}
            </span>
          </p>
        </div>
        <div className="mt-6 flex flex-row">
          <button
            className="px-4 py-2 bg-blue-600 text-white rounded-lg shadow-md hover:bg-blue-700 transition mr-5"
            onClick={() => navigate(-1)}
          >
            Quay Lại
          </button>
          <button
            className="px-4 py-2 bg-green-600 text-white rounded-lg shadow-md hover:bg-green-700 transition"
            onClick={handleEditClick}
          >
            Chỉnh sửa
          </button>
        </div>

        {/* Popup Chỉnh sửa */}
        {isEditing && (
          <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center">
            <div className="bg-white p-6 rounded-lg shadow-lg w-96">
              <h3 className="text-xl font-semibold mb-4">Chỉnh sửa Hội Thảo</h3>
              <input
                type="text"
                name="name"
                value={editData.name}
                onChange={handleInputChange}
                className="w-full p-2 border rounded mb-3"
                placeholder="Tên Hội Thảo"
              />
              <textarea
                name="description"
                value={editData.description}
                onChange={handleInputChange}
                className="w-full p-2 border rounded mb-3"
                placeholder="Mô Tả"
              />
              <input
                type="datetime-local"
                name="startDate"
                value={editData.startDate}
                onChange={handleInputChange}
                className="w-full p-2 border rounded mb-3"
              />
              <input
                type="datetime-local"
                name="endDate"
                value={editData.endDate}
                onChange={handleInputChange}
                className="w-full p-2 border rounded mb-3"
              />
              <input
                type="text"
                name="location"
                value={editData.location}
                onChange={handleInputChange}
                className="w-full p-2 border rounded mb-3"
                placeholder="Địa Điểm"
              />
              <div className="flex justify-end space-x-3">
                <button
                  className="px-4 py-2 bg-gray-400 text-white rounded-lg shadow-md hover:bg-gray-500 transition"
                  onClick={() => setIsEditing(false)}
                >
                  Hủy
                </button>
                <button
                  className="px-4 py-2 bg-blue-600 text-white rounded-lg shadow-md hover:bg-blue-700 transition"
                  onClick={handleSaveChanges}
                >
                  Lưu
                </button>
              </div>
            </div>
          </div>
        )}
      </div>
      {/* Danh sách đăng ký */}
      <div className="mt-6">
        <h3 className="text-xl font-semibold text-gray-700 mb-3">
          Danh Sách Đại Biểu
        </h3>
        <button
          onClick={() => handleOpenPopup(id)}
          className="bg-purple-500 text-white px-2 py-1 rounded mb-3"
        >
          Thêm Đại Biểu Tham gia Hội Thảo
        </button>
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
                  <td
                    className="border border-gray-300 px-4 py-2"
                    onClick={() =>
                      navigate(
                        `/admin/delegate-details/${delegate.delegateId}`,
                        {
                          state: { registerName: delegate.delegateName },
                        }
                      )
                    }
                  >
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
                  <select
                    className="border p-1 rounded"
                    value={delegate.status || "Pending"}
                    onChange={(e) =>
                      handleUpdateStatus(delegate.id, e.target.value)
                    }
                  >
                    <option value="Pending">Pending</option>
                    <option value="Confirmed">Confirmed</option>
                    <option value="Cancelled">Cancelled</option>
                  </select>
                </tr>
              ))}
            </tbody>
          </table>
        ) : (
          <p className="text-gray-500">Chưa có đại biểu nào đăng ký.</p>
        )}
      </div>
      {showPopup && (
        <div className="fixed inset-0 flex items-center justify-center bg-gray-900 bg-opacity-50">
          <div className="bg-white p-6 rounded shadow-lg w-96">
            <h3 className="text-lg font-semibold mb-4">
              Thêm Đại Biểu Vào Hội Thảo
            </h3>
            <label className="block mb-2">
              Email Đại Biểu
              <input
                type="email"
                value={delegateEmail}
                onChange={(e) => setDelegateEmail(e.target.value)}
                className="border p-2 w-full rounded"
              />
            </label>
            <button
              onClick={handleSearchDelegate}
              className="bg-blue-500 text-white px-4 py-2 rounded mb-4"
            >
              Tìm Đại Biểu
            </button>

            {searchDelegates.length > 0 && (
              <label className="block mb-2">
                Chọn Đại Biểu
                <select
                  value={delegateId}
                  onChange={(e) => setDelegateId(e.target.value)}
                  className="border p-2 w-full rounded"
                >
                  <option value="">Chọn đại biểu</option>
                  {searchDelegates.map((delegate) => (
                    <option key={delegate.id} value={delegate.id}>
                      {delegate.name} ({delegate.email})
                    </option>
                  ))}
                </select>
              </label>
            )}

            {delegateId && (
              <>
                <label className="block mb-2">
                  Vai Trò
                  <select
                    value={selectedRoleId}
                    onChange={(e) => setSelectedRoleId(e.target.value)}
                    className="border p-2 w-full rounded"
                  >
                    <option value="">Chọn vai trò</option>
                    {conferenceRoles.map((role) => (
                      <option key={role.id} value={role.id}>
                        {role.name}
                      </option>
                    ))}
                  </select>
                </label>

                <label className="block mb-2">
                  Trạng Thái
                  <select
                    value={selectedStatus}
                    onChange={(e) => setSelectedStatus(e.target.value)}
                    className="border p-2 w-full rounded"
                  >
                    <option value="Pending">Pending</option>
                    <option value="Confirmed">Confirmed</option>
                    <option value="Cancelled">Cancelled</option>
                  </select>
                </label>
              </>
            )}
            <div className="flex gap-2 mt-4">
              <button
                onClick={handleClosePopup}
                className="bg-gray-500 text-white px-4 py-2 rounded"
              >
                Hủy
              </button>
              {delegateId && (
                <button
                  onClick={handleAddDelegateToConferences}
                  className="bg-gray-500 text-white px-4 py-2 rounded"
                >
                  Lưu
                </button>
              )}
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ManageConferenceDetails;
