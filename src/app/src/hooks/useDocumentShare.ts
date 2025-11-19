import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
    getShares,
    createShare,
    updateSharePermission,
    deleteShare,
} from "../services/shareService";
import type { ShareForm } from "../types/schemas";

export function useDocumentShare(documentId: string) {
    const queryClient = useQueryClient();

    const { data, isLoading } = useQuery({
        queryKey: ["shares", documentId],
        queryFn: () => getShares(documentId),
    });

    const updatePermissionMutation = useMutation({
        mutationFn: ({ shareId, permission }: { shareId: string; permission: number }) =>
            updateSharePermission(documentId, shareId, permission),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["shares", documentId] });
        },
    });

    const deleteMutation = useMutation({
        mutationFn: (shareId: string) => deleteShare(documentId, shareId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["shares", documentId] });
        },
        onError: (err: any) => {
            console.error("Share error:", err.message);
        },
    });

    const createMutation = useMutation({
        mutationFn: (data: ShareForm) => createShare(documentId, data),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["shares", documentId] });
        },
        onError: (error) => {
            console.error("Share error:", error.message);
        },
    });


    const updatePermission = (shareId: string, permission: number) =>
        updatePermissionMutation.mutateAsync({ shareId, permission });

    const removeShare = (shareId: string) =>
        deleteMutation.mutateAsync(shareId);

    return {
        shares: data ?? [],
        loading: isLoading,
        createShare: createMutation.mutateAsync,
        createError: createMutation.error,
        updatePermission,
        removeShare,
    };
}