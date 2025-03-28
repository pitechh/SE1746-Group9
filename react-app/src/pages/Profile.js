import React, { useEffect, useState } from "react";
import { getRequest, postRequest } from "../../src/services/apiHelper";
import Swal from "sweetalert2";

const Profile = () => {
  const [profile, setProfile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [formData, setFormData] = useState({});
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isPasswordModalOpen, setIsPasswordModalOpen] = useState(false);
  const [passwordData, setPasswordData] = useState({
    currentPassword: "",
    newPassword: "",
    confirmPassword: "",
  });

  useEffect(() => {
    fetchProfile();
  }, []);

  const handleChangePassword = async () => {
    if (passwordData.newPassword !== passwordData.confirmPassword) {
      Swal.fire(
        "Thất bại!",
        "Mật khẩu mới và mật khẩu xác nhận không trùng nhau",
        "warning"
      );
      return;
    }
    setIsSubmitting(true);
    try {
      const response = await postRequest("/Auth/change-password", passwordData);
      if (response.isSuccess) {
        Swal.fire("Thành công!", "Đổi mật khẩu thành công!", "success");
        setIsPasswordModalOpen(false);
      } else {
        Swal.fire(
          "Thất bại!",
          response.Message || "Đổi mật khẩu thất bại",
          "error"
        );
      }
    } catch (error) {
      //alert("Lỗi khi đổi mật khẩu!");
    } finally {
      setIsSubmitting(false);
    }
  };

  const fetchProfile = async () => {
    try {
      const response = await getRequest("/Auth/profile");
      if (response.isSuccess) {
        setProfile(response.data);
        setFormData(response.data);
      } else {
      }
    } catch (error) {
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handlePasswordChange = (e) => {
    setPasswordData({ ...passwordData, [e.target.name]: e.target.value });
  };

  const handleUpdateProfile = async () => {
    setIsSubmitting(true);
    try {
      const response = await postRequest("/Auth/update-profile", formData);
      if (response.isSuccess) {
        Swal.fire("Thành công!", "Cập nhật thành công!", "success");
        setProfile(formData);
        setIsModalOpen(false);
      } else {
        Swal.fire(
          "Thất bại!",
          response.Message || "Cập nhật thất bại!",
          "error"
        );
      }
    } catch (error) {
      Swal.fire("Thất bại!", error.message || "Cập nhật thất bại!", "error");
    } finally {
      setIsSubmitting(false);
    }
  };

  if (loading) {
    return <div className="text-center p-4">Đang tải...</div>;
  }

  if (!profile) {
    return (
      <div className="text-center p-4 text-red-500">Không tìm thấy hồ sơ!</div>
    );
  }

  return (
    <div className="container mx-auto p-6">
      <h2 className="text-2xl font-bold mb-4">Hồ sơ cá nhân</h2>

      <div className="bg-white p-4 shadow rounded-lg">
        {profile.AvatarUrl && (
          <div className="text-center mb-4">
            <img
              src={profile.AvatarUrl}
              alt="Avatar"
              className="w-32 h-32 rounded-full mx-auto"
            />
          </div>
        )}

        <div className="grid grid-cols-2 gap-4">
          <div>
            <p className="font-semibold">Họ tên:</p>
            <p>{profile.fullName}</p>
          </div>
          <div>
            <p className="font-semibold">Email:</p>
            <p>{profile.email}</p>
          </div>
          <div>
            <p className="font-semibold">Số điện thoại:</p>
            <p>{profile.phone}</p>
          </div>
          <div>
            <p className="font-semibold">Tổ chức:</p>
            <p>{profile.organization || "Không có"}</p>
          </div>
          <div>
            <p className="font-semibold">Chức vụ:</p>
            <p>{profile.position || "Không có"}</p>
          </div>
          <div>
            <p className="font-semibold">Giới tính:</p>
            <p>{profile.gender || "Không rõ"}</p>
          </div>
          <div>
            <p className="font-semibold">Ngày sinh:</p>
            <p>{profile.dateOfBirth || "Không có"}</p>
          </div>
          <div>
            <p className="font-semibold">Quốc tịch:</p>
            <p>{profile.nationality || "Không có"}</p>
          </div>
          <div className="col-span-2">
            <p className="font-semibold">Địa chỉ:</p>
            <p>{profile.Address || "Không có"}</p>
          </div>
          <div className="col-span-2">
            <p className="font-semibold">Số hộ chiếu:</p>
            <p>{profile.passportNumber || "Không có"}</p>
          </div>
          <div className="col-span-2">
            <p className="font-semibold">Tiểu sử:</p>
            <p>{profile.biography || "Không có"}</p>
          </div>
        </div>
        <div className="my-5">
          <button
            onClick={() => setIsModalOpen(true)}
            className="px-4 py-2 bg-blue-500 text-white rounded-lg"
          >
            Cập nhật hồ sơ
          </button>
          <button
            onClick={() => setIsPasswordModalOpen(true)}
            className="px-4 py-2 bg-red-500 text-white rounded-lg mx-5"
          >
            Đổi mật khẩu
          </button>
        </div>
      </div>

      {/* Popup (Modal) */}
      {isModalOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center">
          <div className="bg-white p-6 rounded-lg w-1/2 shadow-lg relative">
            <h2 className="text-xl font-bold mb-4 text-center">
              Cập nhật Hồ sơ
            </h2>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="font-semibold">Họ tên:</label>
                <input
                  type="text"
                  name="fullName"
                  value={formData.fullName || ""}
                  onChange={handleInputChange}
                  className="w-full border p-2 rounded"
                />
              </div>
              <div>
                <label className="font-semibold">Email:</label>
                <input
                  type="email"
                  name="email"
                  value={formData.email || ""}
                  onChange={handleInputChange}
                  className="w-full border p-2 rounded"
                  disabled
                />
              </div>
              <div>
                <label className="font-semibold">Số điện thoại:</label>
                <input
                  type="text"
                  name="phone"
                  value={formData.phone || ""}
                  onChange={handleInputChange}
                  className="w-full border p-2 rounded"
                />
              </div>
              <div>
                <label className="font-semibold">Tổ chức:</label>
                <input
                  type="text"
                  name="organization"
                  value={formData.organization || ""}
                  onChange={handleInputChange}
                  className="w-full border p-2 rounded"
                />
              </div>
              <div>
                <label className="font-semibold">Chức vụ:</label>
                <input
                  type="text"
                  name="position"
                  value={formData.position || ""}
                  onChange={handleInputChange}
                  className="w-full border p-2 rounded"
                />
              </div>
              <div>
                <label className="font-semibold">Ngày sinh:</label>
                <input
                  type="date"
                  name="dateOfBirth"
                  value={formData.dateOfBirth || ""}
                  onChange={handleInputChange}
                  className="w-full border p-2 rounded"
                />
              </div>
              <div>
                <label className="font-semibold">Giới tính:</label>
                <select
                  name="gender"
                  value={formData.gender || ""}
                  onChange={handleInputChange}
                  className="w-full border p-2 rounded"
                >
                  <option value="Male">Nam</option>
                  <option value="Female">Nữ</option>
                  <option value="Other">Khác</option>
                </select>
              </div>
            </div>

            {/* Nút Lưu & Đóng */}
            <div className="text-center mt-4">
              <button
                onClick={handleUpdateProfile}
                className={`px-4 py-2 bg-green-500 text-white rounded-lg ${
                  isSubmitting ? "opacity-50 cursor-not-allowed" : ""
                }`}
                disabled={isSubmitting}
              >
                {isSubmitting ? "Đang cập nhật..." : "Lưu thay đổi"}
              </button>
              <button
                onClick={() => setIsModalOpen(false)}
                className="px-4 py-2 bg-gray-500 text-white rounded-lg ml-2"
              >
                Đóng
              </button>
            </div>
          </div>
        </div>
      )}

      {isPasswordModalOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex justify-center items-center">
          <div className="bg-white p-6 rounded-lg w-1/3 shadow-lg relative">
            <h2 className="text-xl font-bold mb-4 text-center">Đổi mật khẩu</h2>
            <div className="mb-4">
              <label className="font-semibold">Mật khẩu hiện tại:</label>
              <input
                required
                type="password"
                name="currentPassword"
                value={passwordData.currentPassword}
                onChange={handlePasswordChange}
                className="w-full border p-2 rounded"
              />
            </div>
            <div className="mb-4">
              <label className="font-semibold">Mật khẩu mới:</label>
              <input
                required
                type="password"
                name="newPassword"
                value={passwordData.newPassword}
                onChange={handlePasswordChange}
                className="w-full border p-2 rounded"
              />
            </div>
            <div className="mb-4">
              <label className="font-semibold">Xác nhận mật khẩu mới:</label>
              <input
                required
                type="password"
                name="confirmPassword"
                value={passwordData.confirmPassword}
                onChange={handlePasswordChange}
                className="w-full border p-2 rounded"
              />
            </div>
            <div className="text-center mt-4">
              <button
                onClick={handleChangePassword}
                className={`px-4 py-2 bg-green-500 text-white rounded-lg ${
                  isSubmitting ? "opacity-50 cursor-not-allowed" : ""
                }`}
                disabled={isSubmitting}
              >
                {isSubmitting ? "Đang xử lý..." : "Đổi mật khẩu"}
              </button>
              <button
                onClick={() => setIsPasswordModalOpen(false)}
                className="px-4 py-2 bg-gray-500 text-white rounded-lg ml-2"
              >
                Đóng
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Profile;
