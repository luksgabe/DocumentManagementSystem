import DocumentList from "../components/DocumentList";
import UploadModal from "../components/UploadModal";
import { useEffect, useState } from "react";
import { useDocuments } from "../hooks/useDocuments";

export default function DocumentsPage() {
  const { documents, loading, upload, download, remove, error } =
    useDocuments();
  const [open, setOpen] = useState(false);

  const [mainError, setMainError] = useState<string | null>(null);

  useEffect(() => {
    if (error) {
      setMainError(error.message);
    } else {
      setMainError(null);
    }
  }, [error]);

  return (
    <div>
      <div className="pb-8">
        <button
          onClick={() => setOpen(true)}
          className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded"
        >
          Upload Document
        </button>
      </div>

      {mainError && <p className="text-red-500 mt-2 text-sm">{mainError}</p>}

      <UploadModal
        isOpen={open}
        onClose={() => setOpen(false)}
        onUpload={upload}
      />

      <DocumentList
        documents={documents}
        onDownload={download}
        onDelete={remove}
        loading={loading}
      />
    </div>
  );
}
