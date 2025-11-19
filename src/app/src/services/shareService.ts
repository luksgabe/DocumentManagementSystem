import api from "./api";
import type { ShareForm } from "../types/schemas";
import type { DocumentShare } from "../types/dtos";

export async function getShares(documentId: string): Promise<DocumentShare[]> {
    const res = await api.get(`/v1/documents/${documentId}/share`);
    return res.data.data;
}

export async function createShare(documentId: string, data: ShareForm): Promise<DocumentShare> {
    try {
        const res = await api.post(`/v1/documents/${documentId}/share`, data);
        return res.data.data;
    } catch (err: any) {
        throw new Error(err.response?.data?.error || "Unknown error");
    }

}

export async function updateSharePermission(
    documentId: string,
    shareId: string,
    permission: number
): Promise<void> {
    try {
        await api.put(`/v1/documents/${documentId}/share/${shareId}`, { permission });
    } catch (err: any) {
        throw new Error(err.response?.data?.error || "Unknown error");
    }
}

export async function deleteShare(documentId: string, shareId: string): Promise<void> {
    try {
        await api.delete(`/v1/documents/${documentId}/share/${shareId}`);
    } catch (err: any) {
        throw new Error(err.response?.data?.error || "Unknown error");
    }
}
