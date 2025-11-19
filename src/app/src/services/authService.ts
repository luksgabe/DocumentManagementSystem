import api from "./api";

export async function loginService(email: string, password: string) {
    const response = await api.post("/v1/auth/login", { email, password });
    return response.data;
}
