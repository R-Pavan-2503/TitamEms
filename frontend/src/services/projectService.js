import api from './api';

const projectService = {

    getMyProjects: async () => {
        const response = await api.get('/projects/my-projects');
        return response.data;
    },


    createProject: async (name, description) => {
        const response = await api.post('/projects', { name, description });
        return response.data;
    },


    assignEmployee: async (projectId, employeeId) => {
        const response = await api.post('/projects/assign', { projectId, employeeId });
        return response.data;
    },

    unassignEmployee: async (projectId, employeeId) => {
        const response = await api.post('/projects/unassign', { projectId, employeeId });
        return response.data;
    },


    getAllProjects: async () => {
        const response = await api.get('/projects');
        return response.data;
    }
};

export default projectService;