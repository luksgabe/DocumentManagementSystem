import { Trash2 } from "lucide-react";
import type { DocumentShare } from "../types/dtos";
import { Permission, TargetType } from "../types/enums";

interface ShareTableProps {
  shares: DocumentShare[];
  loading: boolean;
  onDelete: (id: string) => void;
  onUpdatePermission: (id: string, permission: Permission) => void;
}

export default function ShareTable({
  shares,
  loading,
  onDelete,
  onUpdatePermission,
}: ShareTableProps) {
  if (loading)
    return <p className="text-center text-muted-foreground">Loading...</p>;

  if (!shares?.length)
    return (
      <div className="text-center py-10 bg-card border border-border rounded-lg">
        <h3 className="text-lg font-medium">This document is not shared yet</h3>
        <p className="text-muted-foreground mt-1">
          Click "Share Document" to add someone.
        </p>
      </div>
    );

  return (
    <div className="bg-card border border-border rounded-lg overflow-hidden">
      <table className="w-full">
        <thead className="bg-muted border-b border-border">
          <tr>
            <th className="px-6 py-3 text-left text-sm font-semibold">
              Target
            </th>
            <th className="px-6 py-3 text-left text-sm font-semibold">Type</th>
            <th className="px-6 py-3 text-left text-sm font-semibold">
              Permission
            </th>
            <th className="px-6 py-3 text-left text-sm font-semibold">
              Shared At
            </th>
            <th className="px-6 py-3 text-right text-sm font-semibold">
              Actions
            </th>
          </tr>
        </thead>

        <tbody className="divide-y divide-border">
          {shares.map((s) => (
            <tr key={s.id} className="hover:bg-muted transition">
              <td className="px-6 py-4">{s.targetValue}</td>

              <td className="px-6 py-4">
                {s.targetType === TargetType.USER ? "User" : "Role"}
              </td>

              <td className="px-6 py-4">
                <select
                  value={s.permission}
                  onChange={(e) =>
                    onUpdatePermission(
                      s.id,
                      Number(e.target.value) as Permission
                    )
                  }
                  className="px-3 py-1 rounded-md bg-input border border-border"
                >
                  <option value={Permission.READ}>Read</option>
                  <option value={Permission.WRITE}>Write</option>
                  <option value={Permission.DELETE}>Delete</option>
                  <option value={Permission.SHARE}>Share</option>
                </select>
              </td>

              <td className="px-6 py-4">
                {new Date(s.sharedAt).toLocaleString()}
              </td>

              <td className="px-6 py-4 text-right">
                <button
                  onClick={() => onDelete(s.id)}
                  className="p-2 hover:bg-destructive/10 rounded-md"
                >
                  <Trash2 size={18} className="text-destructive" />
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
