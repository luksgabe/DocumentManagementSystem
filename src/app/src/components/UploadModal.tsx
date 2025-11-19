"use client";

import { useState, useRef } from "react";
import { Upload, X } from "lucide-react";
import type { UseMutateAsyncFunction } from "@tanstack/react-query";
import { AccessType } from "../types/enums";
import z from "zod";

interface UploadModalProps {
  isOpen: boolean;
  onClose: () => void;
  onUpload: UseMutateAsyncFunction<any, Error, FormData, unknown>;
}

type UploadFormState = {
  title: string;
  description: string;
  accessType: AccessType;
  file: File | null;
};

const uploadSchema = z.object({
  title: z.string().min(1, "Title is required"),
  description: z.string().min(1, "Description is required"),
  accessType: z.enum(AccessType, "Access type is required"),
  file: z.instanceof(File, { message: "File is required" }),
});

type UploadFormErrors = Partial<Record<keyof UploadFormState, string>>;

export default function UploadModal({
  isOpen,
  onClose,
  onUpload,
}: UploadModalProps) {
  const [dragActive, setDragActive] = useState(false);

  const [form, setForm] = useState<UploadFormState>({
    title: "",
    description: "",
    accessType: AccessType.PUBLIC ?? 0,
    file: null,
  });

  const [errors, setErrors] = useState<UploadFormErrors>({});

  const fileInputRef = useRef<HTMLInputElement>(null);

  if (!isOpen) return null;

  const updateField = <K extends keyof UploadFormState>(
    field: K,
    value: UploadFormState[K]
  ) => {
    setForm((prev) => ({ ...prev, [field]: value }));
    setErrors((prev) => ({ ...prev, [field]: undefined }));
  };

  const handleDrag = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(e.type === "dragenter" || e.type === "dragover");
  };

  const handleDrop = (e: React.DragEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setDragActive(false);

    if (e.dataTransfer.files?.length > 0) {
      updateField("file", e.dataTransfer.files[0]);
    }
  };

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files?.length! > 0) {
      updateField("file", e.target.files![0]);
    }
  };

  const handleSubmit = async () => {
    try {
      const parsed = uploadSchema.parse({
        ...form,
        file: form.file,
      });

      const formData = new FormData();
      formData.append("title", parsed.title);
      formData.append("description", parsed.description);
      formData.append("accessType", String(parsed.accessType));
      formData.append("file", parsed.file);

      await onUpload(formData);

      // reset
      setForm({
        title: "",
        description: "",
        accessType: AccessType.PUBLIC ?? 1,
        file: null,
      });
      setErrors({});
      onClose();
    } catch (err: any) {
      if (err instanceof z.ZodError) {
        const fieldErrors: UploadFormErrors = {};
        for (const issue of err.issues) {
          const field = issue.path[0] as keyof UploadFormState;
          if (!fieldErrors[field]) {
            fieldErrors[field] = issue.message;
          }
        }
        setErrors(fieldErrors);
      }
    }
  };

  return (
    <div className="fixed inset-0 bg-black/50 flex items-center justify-center p-4 z-50">
      <div className="bg-card rounded-lg shadow-xl max-w-md w-full">
        {/* Header */}
        <div className="flex items-center justify-between p-6 border-b border-border">
          <h2 className="text-xl font-bold text-foreground">Upload Document</h2>
          <button
            onClick={onClose}
            className="p-1 hover:bg-muted rounded-md transition-colors duration-200"
          >
            <X size={20} className="text-muted-foreground" />
          </button>
        </div>

        {/* Body */}
        <div className="p-6 space-y-4">
          {/* Title */}
          <div>
            <label className="block text-sm font-medium text-foreground mb-1">
              Title
            </label>
            <input
              type="text"
              value={form.title}
              onChange={(e) => updateField("title", e.target.value)}
              className="w-full px-4 py-2 bg-input border border-border rounded-md focus:ring-2 focus:ring-primary text-foreground"
            />
            {errors.title && (
              <p className="mt-1 text-xs text-destructive">{errors.title}</p>
            )}
          </div>

          {/* Description */}
          <div>
            <label className="block text-sm font-medium text-foreground mb-1">
              Description
            </label>
            <textarea
              value={form.description}
              onChange={(e) => updateField("description", e.target.value)}
              className="w-full px-4 py-2 bg-input border border-border rounded-md focus:ring-2 focus:ring-primary text-foreground"
            />
            {errors.description && (
              <p className="mt-1 text-xs text-destructive">
                {errors.description}
              </p>
            )}
          </div>

          {/* Access Type */}
          <div>
            <label className="block text-sm font-medium text-foreground mb-1">
              Access Type
            </label>
            <select
              value={form.accessType}
              onChange={(e) =>
                updateField("accessType", Number(e.target.value) as AccessType)
              }
              className="w-full px-4 py-2 bg-input border border-border rounded-md focus:ring-2 focus:ring-primary text-foreground"
            >
              <option value={AccessType.PUBLIC}>Public</option>
              <option value={AccessType.PRIVATE}>Private</option>
              <option value={AccessType.RESTRICTED}>Restricted</option>
            </select>
            {errors.accessType && (
              <p className="mt-1 text-xs text-destructive">
                {errors.accessType}
              </p>
            )}
          </div>

          {/* File upload */}
          {!form.file ? (
            <div
              onDragEnter={handleDrag}
              onDragLeave={handleDrag}
              onDragOver={handleDrag}
              onDrop={handleDrop}
              className={`border-2 border-dashed rounded-lg p-8 text-center transition-colors duration-200 ${
                dragActive
                  ? "border-primary bg-primary/5"
                  : "border-border hover:border-primary/50"
              }`}
            >
              <Upload
                size={32}
                className="mx-auto text-muted-foreground mb-3"
              />
              <p className="text-foreground font-medium mb-1">
                Drag and drop your file
              </p>
              <p className="text-sm text-muted-foreground mb-4">or</p>
              <button
                onClick={() => fileInputRef.current?.click()}
                className="text-primary hover:text-primary/80 font-medium transition-colors duration-200"
              >
                Browse files
              </button>
              <input
                ref={fileInputRef}
                type="file"
                onChange={handleFileChange}
                className="hidden"
              />
              {errors.file && (
                <p className="mt-2 text-xs text-destructive">{errors.file}</p>
              )}
            </div>
          ) : (
            <div className="text-center">
              <div className="w-12 h-12 bg-primary/10 rounded-lg flex items-center justify-center mx-auto mb-3">
                <span className="text-2xl">📄</span>
              </div>
              <p className="text-foreground font-medium">{form.file.name}</p>
              <p className="text-sm text-muted-foreground mb-4">
                {(form.file.size / 1024 / 1024).toFixed(2)} MB
              </p>
              <button
                onClick={() => updateField("file", null)}
                className="text-sm text-muted-foreground hover:text-foreground transition-colors duration-200"
              >
                Change file
              </button>
            </div>
          )}
        </div>

        {/* Footer */}
        <div className="flex gap-3 p-6 border-t border-border">
          <button
            onClick={onClose}
            className="flex-1 px-4 py-2 bg-muted text-foreground rounded-md hover:bg-muted/80 transition-colors duration-200 font-medium"
          >
            Cancel
          </button>
          <button
            onClick={handleSubmit}
            disabled={!form.file}
            className="flex-1 px-4 py-2 bg-primary hover:bg-primary/90 disabled:opacity-50 disabled:cursor-not-allowed text-primary-foreground rounded-md transition-colors duration-200 font-medium"
          >
            Upload
          </button>
        </div>
      </div>
    </div>
  );
}
