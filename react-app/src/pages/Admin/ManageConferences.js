import { useEffect, useState } from "react";
import { getRequest, postRequest } from "../../services/apiHelper";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";

const ManageConferences = () => {
  const [conferences, setConferences] = useState([]);
  const [showForm, setShowForm] = useState(false);
  const [formData, setFormData] = useState({
    id: "",
    name: "",
    description: "",
    startDate: "",
    endDate: "",
    location: "",
  });
  const [editingConference, setEditingConference] = useState(null);
  const [searchName, setSearchName] = useState("");
  const navigate = useNavigate(); // Sử dụng điều hướng

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
      //alert("Lỗi khi tải danh sách hội thảo!");
    }
  };

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleNavigateToDetails = (id) => {
    navigate(`/admin/conference/${id}`); // Điều hướng đến trang chi tiết
  };

  const handleNavigateToConferenceHostingRegistrationList = (id) => {
    navigate(`/admin/conference-hosting-registration-list`);
  };

  const resetForm = () => {
    setFormData({
      name: "",
      description: "",
      startDate: "",
      endDate: "",
      location: "",
    });
    setEditingConference(null);
  };

  const handleCancel = () => {
    resetForm();
    setShowForm(false);
  };

  const handleAddNew = () => {
    resetForm();
    setShowForm(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      if (editingConference) {
        await postRequest(`/Conferences/update?id=${formData.id}`, formData);
      } else {
        await postRequest("/Conferences/create", formData);
      }
      fetchConferences();
      setShowForm(false);
      resetForm();
    } catch (error) {
      //alert("Lưu thông tin thất bại!");
    }
  };

  const handleEdit = (conference) => {
    setEditingConference(conference);
    setFormData(conference);
    setShowForm(true);
  };

  const handleDelete = async (id) => {
    Swal.fire({
      title: "Bạn có chắc chắn muốn xóa hội thảo này?",
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
          await postRequest(`/Conferences/delete?id=${id}`);
          Swal.fire("Đã xóa!", "Hội thảo đã được xóa thành công.", "success");
          fetchConferences();
        } catch (error) {
          Swal.fire("Lỗi!", "Xóa hội thảo thất bại.", "error");
        }
      }
    });
  };

  return (
    <div className="container mx-auto p-6">
      <h2 className="text-2xl font-bold mb-4">Quản lý Hội Thảo</h2>
      <button
        onClick={handleAddNew}
        className="bg-green-500 text-white px-4 py-2 rounded mb-4"
      >
        Thêm Mới
      </button>

      <button
        onClick={handleNavigateToConferenceHostingRegistrationList}
        className="bg-purple-500 text-white px-4 py-2 rounded mb-4 mx-5"
      >
        Xem danh sách các đơn đăng kí tổ chức hội thảo
      </button>

      {showForm && (
        <form
          onSubmit={handleSubmit}
          className="mb-6 bg-white p-4 rounded shadow"
        >
          <h3 className="text-xl font-semibold mb-2">
            {formData.id ? "Cập nhật Hội Thảo" : "Thêm Hội Thảo"}
          </h3>
          <div className="grid grid-cols-2 gap-4">
            <label>
              Tên Hội Thảo
              <input
                type="text"
                name="name"
                value={formData.name}
                onChange={handleChange}
                className="border p-2 rounded"
                required
              />
            </label>
            <label>
              Mô Tả
              <input
                type="text"
                name="description"
                value={formData.description}
                onChange={handleChange}
                className="border p-2 rounded"
              />
            </label>
            <label>
              Ngày Bắt Đầu
              <input
                type="datetime-local"
                name="startDate"
                value={formData.startDate}
                onChange={handleChange}
                className="border p-2 rounded"
                required
              />
            </label>
            <label>
              Ngày Kết Thúc
              <input
                type="datetime-local"
                name="endDate"
                value={formData.endDate}
                onChange={handleChange}
                className="border p-2 rounded"
                required
              />
            </label>
            <label>
              Địa Điểm
              <input
                type="text"
                name="location"
                value={formData.location}
                onChange={handleChange}
                className="border p-2 rounded"
                required
              />
            </label>
          </div>
          <div className="mt-4 flex gap-2">
            <button
              type="submit"
              className="bg-blue-500 text-white px-4 py-2 rounded"
            >
              {editingConference ? "Cập nhật" : "Thêm mới"}
            </button>
            <button
              type="button"
              onClick={handleCancel}
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
        value={searchName}
        onChange={handleSearchChange}
        placeholder="Tìm kiếm theo tên hội thảo ..."
        className="border p-2 rounded w-full mb-4"
      />

      <table className="w-full border-collapse border border-gray-300">
        <thead>
          <tr className="bg-gray-200">
            <th className="border p-2">Tên Hội Thảo</th>
            <th className="border p-2">Ngày Bắt Đầu</th>
            <th className="border p-2">Ngày Kết Thúc</th>
            <th className="border p-2">Địa Điểm</th>
            <th className="border p-2">Thao Tác</th>
          </tr>
        </thead>
        <tbody>
          {conferences.map((conf) => (
            <tr key={conf.id} className="border">
              <td className="border p-2">{conf.name}</td>
              <td className="border p-2">{conf.startDate}</td>
              <td className="border p-2">{conf.endDate}</td>
              <td className="border p-2">{conf.location}</td>
              <td className="border p-2">
                <button
                  onClick={() => handleEdit(conf)}
                  className="bg-yellow-500 text-white px-2 py-1 rounded mr-2"
                >
                  Sửa
                </button>
                <button
                  onClick={() => handleDelete(conf.id)}
                  className="bg-red-500 text-white px-2 py-1 rounded mr-2"
                >
                  Xóa
                </button>
                <button
                  onClick={(e) => {
                    e.preventDefault();
                    handleNavigateToDetails(conf.id);
                  }}
                  className="bg-blue-500 text-white px-2 py-1 rounded"
                >
                  Xem chi tiết
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ManageConferences;
