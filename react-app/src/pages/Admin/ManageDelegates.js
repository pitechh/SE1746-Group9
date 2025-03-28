import React, { useEffect, useState } from "react";
import { getRequest, postRequest } from "../../services/apiHelper";
import Swal from "sweetalert2";
import { useNavigate } from "react-router-dom";

const ManageDelegates = () => {
  const navigate = useNavigate();
  const [delegates, setDelegates] = useState([]);
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({
    id: "",
    fullName: "",
    email: "",
    phone: "",
    password: "",
    organization: "",
    position: "",
    gender: "",
    dateOfBirth: "",
    nationality: "",
    address: "",
    passportNumber: "",
    avatarUrl: "",
    biography: "",
  });

  useEffect(() => {
    fetchDelegates();
  }, []);

  const [searchEmail, setSearchEmail] = useState("");

  const handleSearchChange = (e) => {
    setSearchEmail(e.target.value);
    fetchDelegates(e.target.value);
  };

  const fetchDelegates = async (email = "") => {
    try {
      const url = email
        ? `/Delegates/get-by-email?email=${email}`
        : "/Delegates/get-all";
      const response = await getRequest(url);
      setDelegates(response.data || []);
    } catch (error) {
      //("Lỗi khi tải danh sách delegates!");
    }
  };

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    let response;
    try {
      if (formData.id) {
        response = await postRequest("/Delegates/update", formData);
      } else {
        const addnewDelegate = {
          fullName: formData.fullName,
          email: formData.email,
          phone: formData.phone,
          password: formData.password,
          organization: formData.organization,
          position: formData.position,
          gender: formData.gender,
          dateOfBirth: formData.dateOfBirth,
          nationality: formData.nationality,
          address: formData.address,
          passportNumber: formData.passportNumber,
          avatarUrl: formData.avatarUrl,
          biography: formData.biography,
        };
        response = await postRequest("/Delegates/create", addnewDelegate);
      }

      if (response.isSuccess) {
        Swal.fire("Thành công!", "Thành công.", "success");
      }

      fetchDelegates();
      setShowForm(false);
      resetForm();
    } catch (error) {
      //alert(response?.message || "Failed to do action");
    }
  };

  const handleEdit = (delegate) => {
    setFormData(delegate);
    setShowForm(true);
  };

  const handleDelete = async (id) => {
    Swal.fire({
      title: "Bạn có chắc chắn muốn xóa đại biểu này?",
      text: "Hành động này không thể hoàn tác!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
      confirmButtonText: "Xóa",
      cancelButtonText: "Hủy",
    }).then(async (result) => {
      if (result.isConfirmed) {
        try {
          await postRequest(`/Delegates/delete?id=${id}`);
          Swal.fire("Đã xóa!", "Đại biểu đã được xóa thành công.", "success");
          fetchDelegates();
        } catch (error) {
          Swal.fire("Lỗi!", "Xóa đại biểu thất bại.", "error");
        }
      }
    });
  };

  const resetForm = () => {
    setFormData({
      id: "",
      fullName: "",
      email: "",
      phone: "",
      password: "",
      organization: "",
      position: "",
      gender: "",
      dateOfBirth: "",
      nationality: "",
      address: "",
      passportNumber: "",
      avatarUrl: "",
      biography: "",
    });
  };

  return (
    <div className="container mx-auto p-6">
      <h2 className="text-2xl font-bold mb-4">Quản lý Đại biểu</h2>
      <button
        onClick={() => setShowForm(true)}
        className="bg-green-500 text-white px-4 py-2 rounded mb-4"
      >
        Thêm Mới
      </button>

      {showForm && (
        <form
          onSubmit={handleSubmit}
          className="mb-6 bg-white p-4 rounded shadow"
        >
          <h3 className="text-xl font-semibold mb-2">
            {formData.id ? "Cập nhật Delegate" : "Thêm Delegate"}
          </h3>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label>Họ tên</label>
              <input
                type="text"
                name="fullName"
                value={formData.fullName}
                onChange={handleChange}
                className="border p-2 rounded w-full"
                required
              />
            </div>
            <div>
              <label>Email</label>
              <input
                type="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                className="border p-2 rounded w-full"
                required
              />
            </div>
            <div>
              <label>Số điện thoại</label>
              <input
                type="text"
                name="phone"
                value={formData.phone}
                onChange={handleChange}
                className="border p-2 rounded w-full"
                required
              />
            </div>
            <div>
              <label>Mật khẩu</label>
              <input
                type="password"
                name="password"
                value={formData.password}
                onChange={handleChange}
                className="border p-2 rounded w-full"
                required={!formData.id}
              />
            </div>
            <div>
              <label>Tổ chức</label>
              <input
                type="text"
                name="organization"
                value={formData.organization}
                onChange={handleChange}
                className="border p-2 rounded w-full"
              />
            </div>
            <div>
              <label>Chức vụ</label>
              <input
                type="text"
                name="position"
                value={formData.position}
                onChange={handleChange}
                className="border p-2 rounded w-full"
              />
            </div>
            <div>
              <label>Giới tính</label>
              <select
                name="gender"
                value={formData.gender}
                onChange={handleChange}
                className="border p-2 rounded w-full"
              >
                <option value="Male" defaultChecked>
                  Nam
                </option>
                <option value="Female">Nữ</option>
                <option value="Other">Khác</option>
              </select>
            </div>
            <div>
              <label>Ngày sinh</label>
              <input
                type="date"
                name="dateOfBirth"
                required
                value={formData.dateOfBirth}
                onChange={handleChange}
                className="border p-2 rounded w-full"
              />
            </div>
            <div>
              <label>Quốc tịch</label>
              <input
                type="text"
                name="nationality"
                value={formData.nationality}
                onChange={handleChange}
                className="border p-2 rounded w-full"
              />
            </div>
            <div>
              <label>Địa chỉ</label>
              <input
                type="text"
                name="address"
                value={formData.address}
                onChange={handleChange}
                className="border p-2 rounded w-full"
              />
            </div>
          </div>
          <div className="mt-4 flex gap-2">
            <button
              type="submit"
              className="bg-blue-500 text-white px-4 py-2 rounded"
            >
              {formData.id ? "Cập nhật" : "Thêm mới"}
            </button>
            <button
              type="button"
              onClick={() => setShowForm(false)}
              className="bg-gray-500 text-white px-4 py-2 rounded"
            >
              Hủy
            </button>
          </div>
        </form>
      )}

      {/* Thanh tìm kiếm */}
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
                <td className="p-2 flex justify-around">
                  <button
                    onClick={() => handleEdit(delegate)}
                    className="bg-yellow-500 text-white px-3 py-1 rounded mr-2"
                  >
                    Sửa
                  </button>
                  <button
                    onClick={() => handleDelete(delegate.id)}
                    className="bg-red-500 text-white px-3 py-1 rounded"
                  >
                    Xóa
                  </button>
                  <button
                    className="bg-green-500 text-white px-3 py-1 rounded"
                    onClick={() =>
                      navigate(`/admin/delegate-details/${delegate.id}`, {
                        state: { registerName: delegate.delegateName },
                      })
                    }
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
    </div>
  );
};

export default ManageDelegates;
