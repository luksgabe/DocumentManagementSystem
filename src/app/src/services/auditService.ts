import api from "./api";

export function getAuditLogs(page = 1, pageSize = 50) {
    return api
        .get(`/v1/audit?page=${page}&pageSize=${pageSize}`)
        .then((response) => response.data.data.map((log: any) => {
            return {
                ...log,
                userId: log.userSub,
                timestamp: log.whenUtc,
                details: `Metadata: ${log.metadata}, IP: ${log.ip}, Entity: ${log.entity} (${log.entityId})`,
            };
        }));

}
