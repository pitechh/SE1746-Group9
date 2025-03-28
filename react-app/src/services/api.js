import axios from "axios";

const BASE_URL = "http://localhost:7171/api";

// Tạo instance của axios
const api = axios.create({
  baseURL: BASE_URL,
  headers: {
    "Content-Type": "application/json, charset=UTF-8",
  },
});

// Thêm Authorization Header TỰ ĐỘNG (trừ khi gọi Login)
api.interceptors.request.use(
  (config) => {
    // Nếu API là login, không thêm Authorization
    if (config.url.includes("/Auth/login")) {
      return config;
    }

    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Xử lý lỗi chung
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response) {
      // Nếu bị Unauthorized (401), xóa token & chuyển hướng về login
      if (error.response.status === 401) {
        localStorage.removeItem("token");
        window.location.href = "/login";
      }
    }
    return Promise.reject(error);
  }
);

export default api;
