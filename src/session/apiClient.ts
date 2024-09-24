import axios from "axios";

const apiClient = axios.create({
    baseURL: 'http://localhost:5000/api/session', // Update with your backend URL
    withCredentials: true, // Important for session cookies
});
