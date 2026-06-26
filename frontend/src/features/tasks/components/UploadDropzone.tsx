'use client';

import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { collaborationApi } from '../api/collaboration.api';
import { useQueryClient } from '@tanstack/react-query';
import { COLLAB_KEYS } from '../hooks/useCollaborationHooks';
import { UploadCloud } from 'lucide-react';
import axios from 'axios';

export const UploadDropzone = ({ taskId }: { taskId: string }) => {
    const [isUploading, setIsUploading] = useState(false);
    const queryClient = useQueryClient();

    const handleFileUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0];
        if (!file) return;

        try {
            setIsUploading(true);

            // 1. Request Presigned URL from Velyo Backend
            const { attachmentId, presignedUrl } = await collaborationApi.requestUploadUrl({
                taskId,
                fileName: file.name,
                contentType: file.type,
                fileSizeBytes: file.size
            });

            // 2. Upload file directly to S3 (Bypass Velyo API for performance)
            // We use raw axios here because apiClient adds Auth Headers which S3 will reject
            await axios.put(presignedUrl, file, {
                headers: { 'Content-Type': file.type }
            });

            // 3. Confirm upload with Backend
            await collaborationApi.completeUpload(taskId, { attachmentId });

            // 4. Invalidate UI
            queryClient.invalidateQueries({ queryKey: COLLAB_KEYS.attachments(taskId) });

        } catch (error) {
            console.error("Upload failed", error);
            // In a real app, fire a Toast here.
        } finally {
            setIsUploading(false);
        }
    };

    return (
        <div className="border-2 border-dashed border-zinc-200 dark:border-zinc-800 rounded-xl p-8 text-center hover:bg-zinc-50 dark:hover:bg-zinc-900/50 transition-colors relative">
            <input 
                type="file" 
                onChange={handleFileUpload} 
                className="absolute inset-0 w-full h-full opacity-0 cursor-pointer"
                disabled={isUploading}
            />
            <UploadCloud className="mx-auto h-8 w-8 text-zinc-400 mb-3" />
            <h3 className="text-sm font-medium dark:text-zinc-200">
                {isUploading ? 'Uploading securely...' : 'Click or drag file to this area to upload'}
            </h3>
            <p className="text-xs text-zinc-500 mt-1">Maximum file size 50 MB.</p>
        </div>
    );
};