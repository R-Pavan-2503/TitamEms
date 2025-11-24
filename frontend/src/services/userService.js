import api from "./api";

const userService = {

    getAllEmployees: async () => {
        const response = await api.get('/users/employees');

        return response.data;
    },

    updateMyUsername: async (userName) => {
        const response = await api.put('/users/me/username', {
            userName
        })
        return response.data;
    }
}

export default userService;