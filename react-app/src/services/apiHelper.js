import Swal from "sweetalert2";
import api from "./api";

// Hàm gọi GET API
export const getRequest = async (url, params = {}) => {
  try {
    const response = await api.get(url, { params });
    return response.data;
  } catch (error) {
    console.error("GET Error:", error.response?.data || error.message);
    throw error;
  }
};

// Hàm gọi POST API
export const postRequest = async (url, data) => {
  try {
    const response = await api.post(url, data);
    if (response.isSuccess) {
      Swal.fire({
        title: "Thành công!",
        text: response.data.message || "Thực hiện thành công",
        icon: "success",
        confirmButtonText: "OK",
      });
    }
    return response.data;
  } catch (error) {
    console.error("POST Error:", error.response?.data.message || error.message);
    Swal.fire({
      title: "Lỗi khi thực hiện yêu cầu!",
      text: error.response?.data.message || "Lỗi khi thực hiện yêu cầu!",
      icon: "error",
      confirmButtonText: "OK",
    });
    throw error;
  }
};

// Hàm gọi PUT API
export const putRequest = async (url, data) => {
  try {
    const response = await api.put(url, data);
    //alert(response.data.message || "Yêu cầu PUT thành công!");
    return response.data;
  } catch (error) {
    console.error("PUT Error:", error.response?.data || error.message);
    alert(error.response?.data?.message || "Lỗi khi thực hiện PUT!");
    throw error;
  }
};

// Hàm gọi DELETE API
export const deleteRequest = async (url) => {
  try {
    const response = await api.delete(url);
    //alert(response.data.message || "Xóa thành công!");
    return response.data;
  } catch (error) {
    console.error("DELETE Error:", error.response?.data || error.message);
    //alert(error.response?.data?.message || "Lỗi khi thực hiện DELETE!");
    throw error;
  }
};
