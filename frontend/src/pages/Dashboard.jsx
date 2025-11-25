import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import projectService from '../services/projectService';
import authService from '../services/authService';
import userService from '../services/userService';
import CreateProjectModal from '../components/CreateProjectModal';
import AssignEmployeeModal from '../components/AssignEmployeeModal';
import UpdateUsernameModal from '../components/UpdateUsernameModal';
import UnassignEmployeeModal from '../components/UnassignEmployeeModal'; // <--- 1. Import

export default function Dashboard() {
    const [projects, setProjects] = useState([]);
    const [loading, setLoading] = useState(true);
    const [user, setUser] = useState(null);

    // Modal States
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
    const [isAssignModalOpen, setIsAssignModalOpen] = useState(false);
    const [isUnassignModalOpen, setIsUnassignModalOpen] = useState(false); // <--- 2. New State
    const [isUsernameModalOpen, setIsUsernameModalOpen] = useState(false);

    const navigate = useNavigate();

    useEffect(() => {
        const fetchData = async () => {
            try {
                const currentUser = authService.getCurrentUser();
                setUser(currentUser);

                const isUserAdmin = currentUser && (
                    currentUser.role === 'Admin' ||
                    currentUser['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Admin'
                );

                let data;
                if (isUserAdmin) {
                    data = await projectService.getAllProjects();
                } else {
                    data = await projectService.getMyProjects();
                }

                setProjects(data);
            } catch (err) {
                console.error("Failed to fetch data", err);
                if (err.response && err.response.status === 401) {
                    navigate('/login');
                }
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, [navigate]);

    const handleLogout = () => {
        authService.logout();
        navigate('/login');
    };

    const handleCreateProject = async (projectData) => {
        try {
            const newProject = await projectService.createProject(projectData.name, projectData.description);
            setProjects([newProject, ...projects]);
            setIsCreateModalOpen(false);
            alert("Project Created Successfully!");
        } catch (error) {
            console.error("Failed to create project", error);
            alert("Failed to create project.");
        }
    };

    const handleAssignEmployee = async (projectId, employeeId) => {
        try {
            await projectService.assignEmployee(projectId, employeeId);
            alert("Employee Assigned Successfully!");
            setIsAssignModalOpen(false);
        } catch (error) {
            console.error("Failed to assign", error);
            alert("Failed to assign employee.");
        }
    };

    // 3. New Handler: Unassign
    const handleUnassignEmployee = async (projectId, employeeId) => {
        try {
            await projectService.unassignEmployee(projectId, employeeId);
            alert("Employee Removed Successfully!");
            setIsUnassignModalOpen(false);
        } catch (error) {
            console.error("Failed to remove", error);
            alert("Failed to remove employee. (Are they actually assigned to this project?)");
        }
    };

    const handleUpdateUsername = async (newUsername) => {
        try {
            await userService.updateMyUsername(newUsername);
            setUser({ ...user, sub: newUsername });
            setIsUsernameModalOpen(false);
            alert("Username Updated Successfully!");
        } catch (error) {
            console.error("Failed to update username", error);
            alert("Failed to update username.");
        }
    };

    const isAdmin = user && (
        user.role === 'Admin' ||
        user['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] === 'Admin'
    );

    if (loading) return <div className="text-center mt-20">Loading...</div>;

    return (
        <div className="min-h-screen bg-gray-100">
            <nav className="bg-white shadow p-4 flex justify-between items-center">
                <div className="flex items-center gap-4">
                    <h1 className="text-xl font-bold text-blue-600">TitanEMS</h1>

                    {user && (
                        <div className="flex items-center gap-2 bg-gray-100 px-3 py-1 rounded-full border">
                            <span className="text-sm font-medium text-gray-700">
                                {user.sub || "User"} ({user.role || "Employee"})
                            </span>
                            <button
                                onClick={() => setIsUsernameModalOpen(true)}
                                className="text-xs text-blue-600 hover:text-blue-800 underline"
                            >
                                Edit
                            </button>
                        </div>
                    )}
                </div>
                <button onClick={handleLogout} className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition">
                    Logout
                </button>
            </nav>

            <main className="p-8 max-w-6xl mx-auto">
                {isAdmin && (
                    <div className="mb-8 p-6 bg-blue-50 border border-blue-200 rounded-lg">
                        <h2 className="text-xl font-bold text-blue-800 mb-4">Admin Controls</h2>
                        <div className="flex gap-4 flex-wrap">
                            <button
                                onClick={() => setIsCreateModalOpen(true)}
                                className="bg-blue-600 text-white px-4 py-2 rounded shadow hover:bg-blue-700"
                            >
                                + Create New Project
                            </button>
                            <button
                                onClick={() => setIsAssignModalOpen(true)}
                                className="bg-green-600 text-white px-4 py-2 rounded shadow hover:bg-green-700"
                            >
                                + Assign Employee
                            </button>
                            {/* 4. New Button: Unassign */}
                            <button
                                onClick={() => setIsUnassignModalOpen(true)}
                                className="bg-red-600 text-white px-4 py-2 rounded shadow hover:bg-red-700"
                            >
                                - Remove Employee
                            </button>
                        </div>
                    </div>
                )}

                <h2 className="text-2xl font-bold mb-6 text-gray-800">
                    {isAdmin ? "All Projects" : "My Projects"}
                </h2>

                {projects.length === 0 ? (
                    <p className="text-gray-500 italic">No projects found.</p>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {projects.map((project) => (
                            <div key={project.id} className="bg-white p-6 rounded-lg shadow hover:shadow-lg transition border-l-4 border-blue-500">
                                <h3 className="text-xl font-bold text-gray-800 mb-2">{project.name}</h3>
                                <p className="text-gray-600 mb-4">{project.description || "No description provided."}</p>
                                <div className="text-xs text-gray-400 font-mono">ID: {project.id}</div>
                            </div>
                        ))}
                    </div>
                )}
            </main>

            {/* Render All Modals */}
            <CreateProjectModal
                isOpen={isCreateModalOpen}
                onClose={() => setIsCreateModalOpen(false)}
                onSubmit={handleCreateProject}
            />
            <AssignEmployeeModal
                isOpen={isAssignModalOpen}
                onClose={() => setIsAssignModalOpen(false)}
                projects={projects}
                onSubmit={handleAssignEmployee}
            />

            {/* 5. Render Unassign Modal */}
            <UnassignEmployeeModal
                isOpen={isUnassignModalOpen}
                onClose={() => setIsUnassignModalOpen(false)}
                projects={projects}
                onSubmit={handleUnassignEmployee}
            />

            <UpdateUsernameModal
                isOpen={isUsernameModalOpen}
                onClose={() => setIsUsernameModalOpen(false)}
                onSubmit={handleUpdateUsername}
            />
        </div>
    );
}