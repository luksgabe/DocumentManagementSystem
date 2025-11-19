import { useEffect, useState } from "react";
import { X } from "lucide-react";
import { z } from "zod";
import { TargetType, Permission } from "../types/enums";

export const shareSchema = z.object({
  targetType: z.enum(TargetType, {
    message: "Invalid target type",
  }),

  targetValue: z
    .string()
    .min(1, "Value is required")
    .max(200, "Value too long"),

  permission: z.enum(Permission, {
    message: "Invalid permission value",
  }),
});

type ShareForm = z.infer<typeof shareSchema>;

interface ShareModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSubmit: (data: ShareForm) => void;
}

export default function ShareModal({
  isOpen,
  onClose,
  onSubmit,
}: ShareModalProps) {
  const [form, setForm] = useState<ShareForm>({
    targetType: TargetType.USER,
    targetValue: "",
    permission: Permission.READ,
  });

  const [errors, setErrors] = useState<Record<string, string>>({});

  if (!isOpen) return null;

  const updateField = (field: keyof ShareForm, value: any) => {
    setForm((prev) => ({ ...prev, [field]: value }));
    setErrors((prev) => ({ ...prev, [field]: "" }));
  };

  const submit = (e: React.FormEvent) => {
    e.preventDefault();
    setErrors({});

    const result = shareSchema.safeParse(form);

    if (!result.success) {
      const fieldErrors: Record<string, string> = {};
      result.error.issues.forEach((err) => {
        if (err.path[0]) fieldErrors[err.path[0] as string] = err.message;
      });
      setErrors(fieldErrors);
      return;
    }

    onSubmit(result.data);
    onClose();

    // reset
    setForm({
      targetType: TargetType.USER,
      targetValue: "",
      permission: Permission.READ,
    });
  };

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
      <div className="bg-card rounded-lg shadow-xl max-w-md w-full">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-border">
          <h2 className="text-xl font-bold text-foreground">Share Document</h2>
          <button
            onClick={onClose}
            className="p-1 hover:bg-muted rounded-md transition-colors"
          >
            <X size={20} className="text-muted-foreground" />
          </button>
        </div>

        <form onSubmit={submit} className="p-6 space-y-4">
          {/* Target Type */}
          <div>
            <label className="block text-sm font-medium mb-2">
              Target Type
            </label>

            <select
              value={form.targetType}
              onChange={(e) =>
                updateField("targetType", Number(e.target.value))
              }
              className="w-full px-3 py-2 bg-input border border-border rounded-md"
            >
              <option value={TargetType.USER}>User</option>
              <option value={TargetType.ROLE}>Role</option>
            </select>
            {errors.targetType && (
              <p className="text-destructive text-sm mt-1">
                {errors.targetType}
              </p>
            )}
          </div>

          {/* Target Value */}
          <div>
            <label className="block text-sm font-medium mb-2">
              Value (Email or Role ID)
            </label>
            <input
              type="text"
              value={form.targetValue}
              onChange={(e) => updateField("targetValue", e.target.value)}
              className="w-full px-3 py-2 bg-input border border-border rounded-md"
            />
            {errors.targetValue && (
              <p className="text-destructive text-sm mt-1">
                {errors.targetValue}
              </p>
            )}
          </div>

          {/* Permission */}
          <div>
            <label className="block text-sm font-medium mb-2">Permission</label>
            <select
              value={form.permission}
              onChange={(e) =>
                updateField("permission", Number(e.target.value))
              }
              className="w-full px-3 py-2 bg-input border border-border rounded-md"
            >
              <option value={Permission.READ}>Read</option>
              <option value={Permission.WRITE}>Write</option>
              <option value={Permission.DELETE}>Delete</option>
              <option value={Permission.SHARE}>Share</option>
            </select>
            {errors.permission && (
              <p className="text-destructive text-sm mt-1">
                {errors.permission}
              </p>
            )}
          </div>

          {/* Actions */}
          <div className="flex gap-3 pt-4">
            <button
              type="button"
              onClick={onClose}
              className="w-full px-4 py-2 bg-muted rounded-md hover:bg-muted/80"
            >
              Cancel
            </button>
            <button
              type="submit"
              className="w-full px-4 py-2 bg-primary text-primary-foreground rounded-md hover:bg-primary/90"
            >
              Share
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
