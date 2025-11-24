import api from "./api";

import { jwtDecode } from 'jwt-decode';

const authService = {

    login: async (email, password) => {
        const response = await api.post('auth/login', {
            email, password
        })

        return response.data;
    },

    register: async (email, password, role) => {
        const response = await api.post('auth/register', {
            email, password, role
        })

        return response.data;
    },

    getCurrentUser: () => {
        try {
            const token = localStorage.getItem('token');
            if (!token) return null;

            return jwtDecode(token);
        }
        catch (err) {
            return null;
        }
    },

    logout: () => {
        localStorage.removeItem('token');
    }
};

export default authService;