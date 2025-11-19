import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";

import ShareModal from "../components/ShareModal";
import ShareTable from "../components/ShareTable";
import { useDocumentShare } from "../hooks/useDocumentShare";

export default function SharingPage() {
  const { id } = useParams();
  const {
    shares,
    loading,
    createShare,
    createError,
    updatePermission,
    removeShare,
  } = useDocumentShare(id!);

  const [open, setOpen] = useState(false);
  const [mainError, setMainError] = useState<string | null>(null);

  useEffect(() => {
    if (createError) {
      setMainError(createError.message);
    } else {
      setMainError(null);
    }
  }, [createError]);

  return (
    <div className="space-y-6">
      <button
        onClick={() => setOpen(true)}
        className="bg-primary text-primary-foreground px-4 py-2 rounded-md hover:bg-primary/90"
      >
        Share Document
      </button>

      {mainError && <p className="text-red-500 mt-2 text-sm">{mainError}</p>}

      <ShareModal
        isOpen={open}
        onClose={() => setOpen(false)}
        onSubmit={(data) => createShare(data)}
      />

      <ShareTable
        shares={shares}
        loading={loading}
        onUpdatePermission={updatePermission}
        onDelete={(shareId) => removeShare(shareId)}
      />
    </div>
  );
}
