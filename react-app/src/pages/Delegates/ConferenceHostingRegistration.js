import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import Swal from "sweetalert2";
import { postRequest } from "../../services/apiHelper";

const ConferenceHostingRegistration = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    name: "",
    startDate: "",
    endDate: "",
    location: "",
    createAt: new Date().toISOString(),
    description: "",
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await postRequest("/ConferenceHostingRegistration/create", formData);
      Swal.fire("Thành công!", "Đăng ký hội thảo thành công!", "success");
      navigate("/conference-hosting-registration-list");
    } catch (error) {
      console.error("Lỗi khi đăng ký:", error);
      Swal.fire("Lỗi!", error.response?.data.message, "error");
    }
  };

  return (
    <div className="max-w-lg mx-auto bg-white p-6 rounded-lg shadow-md">
      <h2 className="text-2xl font-bold text-center mb-4">Đăng ký hội thảo</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <div>
          <label className="block font-medium">Tên hội thảo:</label>
          <input
            type="text"
            name="name"
            value={formData.name}
            onChange={handleChange}
            required
            className="w-full p-2 border rounded"
          />
        </div>

        <div className="grid grid-cols-2 gap-4">
          <div>
            <label className="block font-medium">Ngày bắt đầu:</label>
            <input
              type="date"
              name="startDate"
              value={formData.startDate}
              onChange={handleChange}
              required
              className="w-full p-2 border rounded"
            />
          </div>
          <div>
            <label className="block font-medium">Ngày kết thúc:</label>
            <input
              type="date"
              name="endDate"
              value={formData.endDate}
              onChange={handleChange}
              required
              className="w-full p-2 border rounded"
            />
          </div>
        </div>

        <div>
          <label className="block font-medium">Địa điểm:</label>
          <input
            type="text"
            name="location"
            value={formData.location}
            onChange={handleChange}
            required
            className="w-full p-2 border rounded"
          />
        </div>

        <div>
          <label className="block font-medium">Mô tả:</label>
          <textarea
            name="description"
            value={formData.description}
            onChange={handleChange}
            className="w-full p-2 border rounded"
          />
        </div>

        <div className="flex justify-between">
          <button
            type="button"
            onClick={() => navigate("/conference-hosting-registration-list")}
            className="px-4 py-2 bg-gray-500 text-white rounded"
          >
            Quay lại danh sách
          </button>
          <button
            type="submit"
            className="px-4 py-2 bg-blue-600 text-white rounded"
          >
            Đăng ký hội thảo
          </button>
        </div>
      </form>
    </div>
  );
};

export default ConferenceHostingRegistration;
