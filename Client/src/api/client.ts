import { Api } from "./Api";
import { API_URL } from "@/config.ts";

export const api = new Api<string | null>({
    baseUrl: API_URL,
    securityWorker: (token) =>
        token ? { headers: { Authorization: `Bearer ${token}` } } : {},
});

api.setSecurityData(sessionStorage.getItem("token"));