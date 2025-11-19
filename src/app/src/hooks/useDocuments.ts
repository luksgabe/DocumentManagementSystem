import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
    getDocuments,
    deleteDocument,
    uploadDocument,
    getDownload,
} from "../services/documentService";

export function useDocuments() {
    const queryClient = useQueryClient();

    const documentsQuery = useQuery({
        queryKey: ["documents"],
        queryFn: getDocuments,
    });

    const uploadMutation = useMutation({
        mutationFn: uploadDocument,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["documents"] });
        },
        onError: (err: Error) => {
            console.error("Error uploading document", err);
        },
    });

    const downloadMutation = useMutation({
        mutationFn: getDownload,
    });

    const deleteMutation = useMutation({
        mutationFn: deleteDocument,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["documents"] });
        },
    });

    const download = async (id: string, fileName: string) => {
        const response = await downloadMutation.mutateAsync(id);

        const blob = response.data;

        const url = window.URL.createObjectURL(blob);

        const link = document.createElement("a");
        link.href = url;
        link.download = fileName;
        link.click();
        link.remove();

        window.URL.revokeObjectURL(url);
    };

    const remove = async (id: string) => {
        await deleteMutation.mutateAsync(id);
    };

    return {
        documents: documentsQuery.data ?? [],
        loading: documentsQuery.isLoading,
        error: uploadMutation.error,
        upload: uploadMutation.mutateAsync,
        download,
        remove,
    };
}
