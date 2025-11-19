import type { Document } from "../types/dtos";
import api from "./api";

export function getDocuments() {
    return api.get("/v1/documents").then((response): Document[] => {
        return response.data.data.items;
    });
}

export function getDownload(id: string) {
    return api.get(`/v1/documents/${id}/download`, {
        responseType: "blob",
    });
}

export function deleteDocument(id: string) {
    return api.delete(`/v1/documents/${id}`);
}

export function uploadDocument(formData: FormData) {
    try {
        return api.post("/v1/documents", formData).then((response) => response.data.data);
    } catch (error: any) {
        throw new Error(error.response?.data?.error || "Unknown error");
    }
}
