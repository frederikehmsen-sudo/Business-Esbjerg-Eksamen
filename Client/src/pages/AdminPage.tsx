import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth/AuthProvider";

export default function AdminPage() {
    const { logout } = useAuth();
    const navigate = useNavigate();

    return (
        <div className="app">
            <h1 className="text-2xl font-bold text-amber-300 underline">Admin</h1>
            <button
                className="send-button"
                onClick={() => {
                    logout();
                    navigate("/");
                }}
            >
                Log out
            </button>
        </div>
    );
}