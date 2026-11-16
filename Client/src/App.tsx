import { APITester } from "./APITester";
import "./index.css";


import be_logo from "./assets/be_logo.png";
import {type FormEvent, useState} from "react";
import type { LoginRequest } from "./models/login-request"
import {useAuth} from "./auth/AuthProvider";
import { Navigate, Route, Routes, useNavigate} from "react-router-dom"


const homeFor = (role: string | null) => {
    if (role === "Admin") return "/admin";
    if (role === "Accountant") return "/accountant";
    return "/employee";
};

function LoginPage() {
    const { token, role, login } = useAuth();
    const navigate = useNavigate();
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState("");

    async function handleSubmit(event: FormEvent<HTMLFormElement>) {
        event.preventDefault();

        if (isLoading) return;

        setError("");
        setIsLoading(true);

        const request: LoginRequest = { username, password };

        try {
            const loggedInRole = await login(request);
            setPassword("");
            navigate(homeFor(loggedInRole));
        } catch (error) {
            setError(error instanceof Error ? error.message : "Login failed.");
        } finally {
            setIsLoading(false);
        }
    }


    if (token) return <Navigate to={homeFor(role)} replace />;

    return (
        <div className="app">
            <div className="logo-container">
                <img src={be_logo} alt="be-logo" className="logo bun-logo" />
            </div>

            <h1>Login Page</h1>

            <form className="login-form" onSubmit={handleSubmit} aria-busy={isLoading}>
                <label htmlFor="username">Username </label>
                <input
                    id="username"
                    name="username"
                    autoComplete="username"
                    value={username}
                    onChange={event => setUsername(event.target.value)}
                    disabled={isLoading}
                    required
                />

                <label htmlFor="password">Password </label>
                <input
                    id="password"
                    name="password"
                    type="password"
                    autoComplete="current-password"
                    value={password}
                    onChange={event => setPassword(event.target.value)}
                    disabled={isLoading}
                    required
                />

                {error && <p className="login-error" role="alert">{error}</p>}

                <button className="send-button" type="submit" disabled={isLoading}>
                    {isLoading ? "Logging In…" : "Login"}
                </button>
            </form>
        </div>
    );
}

export function App() {
    return (
        <Routes>
            <Route path="/" element={<LoginPage />} />
            {/*<Route path="/employee" element={<EmployeePage />} />*/}
            {/*<Route path="/admin" element={<AdminPage />} />*/}
            {/*<Route path="/accountant" element={<AccountantPage />} />*/}
            <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    );
}

export default App;
