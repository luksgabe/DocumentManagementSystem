import { useQuery } from "@tanstack/react-query";
import { getAuditLogs } from "../services/auditService";

export function useAuditLogs() {
    const query = useQuery({
        queryKey: ["audit"],
        queryFn: () => getAuditLogs(1, 50),
    });

    return {
        logs: query.data ?? [],
        loading: query.isLoading,
        error: query.error,
    };
}
