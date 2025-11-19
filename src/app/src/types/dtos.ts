import type { Permission } from "./enums";

export interface User {
    id: string;
    email: string;
    name: string;
    role: 'admin' | 'user';
}

export interface Document {
    id: string;
    title: string;
    description: string;
    fileName: string;
    fileSizeBytes: number;
    contentType: string;
    ownerEmail: string;
    createdAtUtc: string;
    updatedAt?: string;
    storageUri: string;
    tags: string[];
}

export interface DocumentShare {
    id: string;
    sharedAt: string;
    targetType: number;
    targetValue: string;
    permission: Permission;
}

export interface AuditLog {
    id: string;
    action: string;
    documentId?: string;
    userId: string;
    userEmail: string;
    timestamp: string;
    details: Record<string, unknown>;
}