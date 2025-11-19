import { useState } from "react";
import { loginService } from "../services/authService";

export function useAuth() {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    async function login(email: string, password: string) {
        try {
            setLoading(true);
            setError("");

            const { data } = await loginService(email, password);

            localStorage.setItem("authToken", data.token);

            return data;
        } catch (err: any) {
            const message =
                err.response?.data?.message || "Login failed. Please try again.";

            setError(message);

            throw new Error(message);
        } finally {
            setLoading(false);
        }
    }

    function logout() {
        localStorage.removeItem("authToken");
        window.location.href = "/login";
    }

    return { login, logout, loading, error };
}
