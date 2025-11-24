import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import projectService from '../services/projectService';

export default function Dashboard() {
    const [projects, setProjects] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    // 1. Fetch Data on Component Mount
    useEffect(() => {
        const fetchProjects = async () => {
            try {
                const data = await projectService.getMyProjects();
                setProjects(data);
            } catch (err) {
                console.error("Failed to fetch projects", err);
                // Optional: specific error handling (e.g., if 401, redirect to login)
            } finally {
                setLoading(false);
            }
        };

        fetchProjects();
    }, []);

    // 2. Logout Logic
    const handleLogout = () => {
        localStorage.removeItem('token');
        navigate('/login');
    };

    if (loading) return <div className="text-center mt-20">Loading...</div>;

    return (
        <div className="min-h-screen bg-gray-100">
            {/* Header */}
            <nav className="bg-white shadow p-4 flex justify-between items-center">
                <h1 className="text-xl font-bold text-blue-600">TitanEMS Dashboard</h1>
                <button
                    onClick={handleLogout}
                    className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600 transition"
                >
                    Logout
                </button>
            </nav>

            {/* Main Content */}
            <main className="p-8">
                <h2 className="text-2xl font-bold mb-6 text-gray-800">My Projects</h2>

                {projects.length === 0 ? (
                    <p className="text-gray-500">You are not assigned to any projects yet.</p>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {projects.map((project) => (
                            <div key={project.id} className="bg-white p-6 rounded-lg shadow hover:shadow-lg transition">
                                <h3 className="text-xl font-bold text-gray-800 mb-2">{project.name}</h3>
                                <p className="text-gray-600">{project.description || "No description provided."}</p>
                                <div className="mt-4 text-sm text-gray-400">
                                    Assigned: {new Date(project.createdOn).toLocaleDateString()}
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </main>
        </div>
    );
}