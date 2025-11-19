"use client";

import { Download, Share2, Trash2 } from "lucide-react";
import type { Document } from "../types/dtos";
import { Link } from "react-router-dom";

interface DocumentListProps {
  documents: Document[];
  onDelete: (id: string) => void;
  onDownload: (id: string, fileName: string) => void;
  loading: boolean;
}

export default function DocumentList({
  documents,
  onDelete,
  onDownload,
}: DocumentListProps) {
  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
    });
  };

  const formatFileSize = (sizeInMB: number) => {
    if (sizeInMB < 1) {
      return `${(sizeInMB * 1024).toFixed(0)} KB`;
    }
    return `${sizeInMB.toFixed(1)} MB`;
  };

  if (documents.length === 0) {
    return (
      <div className="text-center py-12">
        <div className="mx-auto text-muted-foreground mb-4 opacity-50 text-5xl">
          📄
        </div>
        <h3 className="text-lg font-medium text-foreground mb-2">
          No documents yet
        </h3>
        <p className="text-muted-foreground">
          Upload your first document to get started
        </p>
      </div>
    );
  }

  return (
    <div className="grid gap-4">
      {documents.length &&
        documents.map((doc) => (
          <div
            key={doc.id}
            className="bg-card border border-border rounded-lg p-6 hover:shadow-lg transition-shadow duration-200"
          >
            <div className="flex items-start justify-between gap-4">
              {/* Document Info */}
              <div className="flex-1 min-w-0">
                <h3 className="text-lg font-semibold text-foreground truncate mb-1">
                  {doc.title}
                </h3>
                <p className="text-sm text-muted-foreground mb-3 line-clamp-2">
                  {doc.description}
                </p>
                <div className="flex flex-wrap gap-4 text-xs text-muted-foreground">
                  <span>Uploaded: {formatDate(doc.createdAtUtc)}</span>
                  <span>
                    Size: {formatFileSize(doc.fileSizeBytes / 1024 / 1024)}
                  </span>
                  <span>By: {doc.ownerEmail}</span>
                </div>
              </div>

              {/* Actions */}
              <div className="flex gap-2">
                <button
                  className="p-2 hover:bg-muted rounded-md transition-colors duration-200"
                  title="Download"
                  onClick={() => onDownload(doc.id, doc.fileName)}
                >
                  <Download
                    size={18}
                    className="text-muted-foreground hover:text-foreground"
                  />
                </button>
                <Link
                  to={`/documents/${doc.id}/share`}
                  className="p-2 hover:bg-muted rounded-md transition-colors duration-200"
                  title="Share"
                >
                  <Share2
                    size={18}
                    className="text-muted-foreground hover:text-foreground"
                  />
                </Link>
                <button
                  className="p-2 hover:bg-muted rounded-md transition-colors duration-200"
                  title="Delete"
                >
                  <Trash2
                    size={18}
                    className="text-muted-foreground hover:text-destructive"
                    onClick={() => onDelete(doc.id)}
                  />
                </button>
              </div>
            </div>
          </div>
        ))}
    </div>
  );
}
