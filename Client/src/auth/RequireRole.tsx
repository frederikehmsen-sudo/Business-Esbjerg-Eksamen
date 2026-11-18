import type { ReactNode } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "./AuthProvider";

export function RequireRole({ role, children }: { role: string; children: ReactNode }) {
    const { token, role: currentRole } = useAuth();

    if (!token || currentRole !== role) {
        return <Navigate to="/" replace />;
    }

    return children;
}