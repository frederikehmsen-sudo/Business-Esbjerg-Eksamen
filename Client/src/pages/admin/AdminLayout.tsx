import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { useAuth } from "@/auth/AuthProvider";
import { Button, cx } from "@/components/ui";
import be_logo from "@/assets/be_logo.png";

const tabs = [
    { to: "/admin/employees", label: "Employees" },
    { to: "/admin/rules", label: "Holiday rules" },
    { to: "/admin/calendar", label: "Calendar" },
];

export default function AdminLayout() {
    const { logout } = useAuth();
    const navigate = useNavigate();

    function handleLogout() {
        logout();
        navigate("/", { replace: true });
    }

    return (
        <div className="min-h-screen bg-page text-left">
            <header className="border-b border-line bg-surface">
                <div className="mx-auto flex h-16 max-w-6xl items-center justify-between px-6">
                    <div className="flex items-center gap-3">
                        <img src={be_logo} alt="" className="h-8 w-auto" />
                        <span className="font-bold text-brand">Business Esbjerg</span>
                        <span className="rounded-md border border-line px-2 py-0.5 text-xs text-white/60">
                            Admin
                        </span>
                    </div>
                    <Button variant="secondary" onClick={handleLogout}>
                        Log out
                    </Button>
                </div>

                <nav aria-label="Admin sections" className="mx-auto flex max-w-6xl gap-1 px-6">
                    {tabs.map(tab => (
                        <NavLink
                            key={tab.to}
                            to={tab.to}
                            className={({ isActive }) =>
                                cx(
                                    "-mb-px border-b-2 px-4 py-3 text-sm font-semibold transition-colors",
                                    isActive
                                        ? "border-brand text-brand"
                                        : "border-transparent text-white/60 hover:text-white",
                                )
                            }
                        >
                            {tab.label}
                        </NavLink>
                    ))}
                </nav>
            </header>

            <main className="mx-auto max-w-6xl px-6 py-8">
                <Outlet />
            </main>
        </div>
    );
}