import { useState, useEffect } from 'react';
import userService from '../services/userService';

export default function UnassignEmployeeModal({ isOpen, onClose, projects, onSubmit }) {
    const [employees, setEmployees] = useState([]);
    const [selectedProjectId, setSelectedProjectId] = useState('');
    const [selectedEmployeeId, setSelectedEmployeeId] = useState('');

    // Fetch Employees when modal opens
    useEffect(() => {
        if (isOpen) {
            const fetchEmployees = async () => {
                try {
                    const data = await userService.getAllEmployees();
                    setEmployees(data);
                } catch (error) {
                    console.error("Failed to load employees", error);
                }
            };

            fetchEmployees();
            setSelectedProjectId('');
            setSelectedEmployeeId('');
        }
    }, [isOpen]);

    if (!isOpen) return null;

    const handleSubmit = (e) => {
        e.preventDefault();
        if (!selectedProjectId || !selectedEmployeeId) {
            alert("Please select both a project and an employee.");
            return;
        }
        onSubmit(selectedProjectId, selectedEmployeeId);
        onClose();
    };

    return (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white p-6 rounded-lg shadow-xl w-full max-w-md border-t-4 border-red-500">
                <h2 className="text-xl font-bold mb-4 text-red-700">Remove Employee from Project</h2>

                <p className="mb-4 text-sm text-gray-600">
                    Select the project and the employee you wish to remove.
                </p>

                <form onSubmit={handleSubmit}>
                    {/* Project Dropdown */}
                    <div className="mb-4">
                        <label className="block text-gray-700 text-sm font-bold mb-2">Select Project</label>
                        <select
                            className="w-full px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-red-500 bg-white"
                            value={selectedProjectId}
                            onChange={(e) => setSelectedProjectId(e.target.value)}
                            required
                        >
                            <option value="">-- Choose a Project --</option>
                            {projects.map(p => (
                                <option key={p.id} value={p.id}>{p.name}</option>
                            ))}
                        </select>
                    </div>

                    {/* Employee Dropdown */}
                    <div className="mb-6">
                        <label className="block text-gray-700 text-sm font-bold mb-2">Select Employee</label>
                        <select
                            className="w-full px-3 py-2 border rounded focus:outline-none focus:ring-2 focus:ring-red-500 bg-white"
                            value={selectedEmployeeId}
                            onChange={(e) => setSelectedEmployeeId(e.target.value)}
                            required
                        >
                            {employees.length === 0 && <option disabled>Loading employees...</option>}
                            {employees.map(u => (
                                <option key={u.id} value={u.id}>{u.userName || u.email} ({u.role})</option>
                            ))}
                        </select>
                    </div>

                    <div className="flex justify-end gap-2">
                        <button type="button" onClick={onClose} className="px-4 py-2 text-gray-600 hover:text-gray-800">
                            Cancel
                        </button>
                        <button type="submit" className="px-4 py-2 bg-red-600 text-white rounded hover:bg-red-700">
                            Remove Employee
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}