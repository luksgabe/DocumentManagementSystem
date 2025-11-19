import { useAuditLogs } from "../hooks/useAuditLogs";
import AuditLogTable from "../components/AuditLogTable";

export default function AuditLogPage() {
  const { logs, loading } = useAuditLogs();

  return <AuditLogTable logs={logs} loading={loading} />;
}
