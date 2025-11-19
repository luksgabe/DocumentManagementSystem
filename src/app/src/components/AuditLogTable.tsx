"use client";

import { ChevronDown } from "lucide-react";
import { useState } from "react";

interface AuditLog {
  id: string;
  action: string;
  entityId?: string;
  documentTitle?: string;
  userId: string;
  userEmail: string;
  timestamp: string;
  details: Record<string, unknown>;
}

interface AuditLogTableProps {
  logs: AuditLog[];
}

export default function AuditLogTable({ logs }: AuditLogTableProps) {
  const [expandedId, setExpandedId] = useState<string | null>(null);

  const formatDateTime = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
    });
  };

  const getActionColor = (action: string) => {
    switch (action) {
      case "DOCUMENT_UPLOADED":
        return "bg-blue-100 text-blue-800";
      case "DOCUMENT_SHARED":
        return "bg-purple-100 text-purple-800";
      case "DOCUMENT_DOWNLOADED":
        return "bg-green-100 text-green-800";
      case "PERMISSION_UPDATED":
        return "bg-orange-100 text-orange-800";
      case "DOCUMENT_ARCHIVED":
        return "bg-gray-100 text-gray-800";
      case "SHARE_REVOKED":
        return "bg-red-100 text-red-800";
      default:
        return "bg-muted text-foreground";
    }
  };

  if (logs.length === 0) {
    return (
      <div className="text-center py-12 bg-card border border-border rounded-lg">
        <h3 className="text-lg font-medium text-foreground mb-2">
          No audit logs
        </h3>
        <p className="text-muted-foreground">Activity will appear here</p>
      </div>
    );
  }

  return (
    <div className="bg-card border border-border rounded-lg overflow-hidden">
      <div className="divide-y divide-border">
        {logs.length &&
          logs.map((log) => (
            <div
              key={log.id}
              className="hover:bg-muted/50 transition-colors duration-200"
            >
              {/* Main Row */}
              <div className="p-4 flex items-center justify-between">
                <div className="flex-1 flex items-center gap-4">
                  {/* Action Badge */}
                  <div
                    className={`px-3 py-1 rounded-full text-xs font-semibold whitespace-nowrap ${getActionColor(
                      log.action
                    )}`}
                  >
                    {log.action.replace(/_/g, " ")}
                  </div>

                  {/* Details */}
                  <div className="flex-1 min-w-0">
                    <p className="text-sm font-medium text-foreground truncate">
                      {log.documentTitle || "System Activity"}
                    </p>
                    <p className="text-xs text-muted-foreground">
                      by {log.userEmail} at {formatDateTime(log.timestamp)}
                    </p>
                  </div>
                </div>

                {/* Expand Button */}
                <button
                  onClick={() =>
                    setExpandedId(expandedId === log.id ? null : log.id)
                  }
                  className="p-1 hover:bg-muted rounded-md transition-colors duration-200"
                >
                  <ChevronDown
                    size={18}
                    className={`text-muted-foreground transition-transform duration-200 ${
                      expandedId === log.id ? "rotate-180" : ""
                    }`}
                  />
                </button>
              </div>

              {/* Expanded Details */}
              {expandedId === log.id && (
                <div className="bg-muted/30 px-4 py-4 border-t border-border">
                  <div className="space-y-2">
                    <div className="grid grid-cols-2 gap-4 text-sm">
                      <div>
                        <span className="text-muted-foreground">Log ID:</span>
                        <p className="font-mono text-foreground">{log.id}</p>
                      </div>
                      <div>
                        <span className="text-muted-foreground">User ID:</span>
                        <p className="font-mono text-foreground">
                          {log.userId}
                        </p>
                      </div>
                    </div>

                    {log.entityId && (
                      <div>
                        <span className="text-muted-foreground text-sm">
                          Document ID:
                        </span>
                        <p className="font-mono text-foreground text-sm">
                          {log.entityId}
                        </p>
                      </div>
                    )}

                    <div>
                      <span className="text-muted-foreground text-sm block mb-2">
                        Additional Details:
                      </span>
                      <pre className="bg-background border border-border rounded p-2 text-xs text-foreground overflow-x-auto">
                        {JSON.stringify(log.details, null, 2)}
                      </pre>
                    </div>
                  </div>
                </div>
              )}
            </div>
          ))}
      </div>
    </div>
  );
}
