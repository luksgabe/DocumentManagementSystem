import { createBrowserRouter, RouterProvider } from "react-router-dom";
import LoginPage from "../pages/LoginPage";
import { ProtectedRoute } from "./ProtectedRoute";
import { PublicRoute } from "./PublicRoute";
import NotFoundPage from "../pages/NotFoundPage";
import DocumentsPage from "../pages/DocumentsPage";
import AppLayout from "../layouts/AppLayout";
import AuditLogPage from "../pages/AuditLogPage";
import SharingPage from "../pages/SharingPage";

const router = createBrowserRouter([
  {
    path: "/login",
    element: (
      <PublicRoute>
        <LoginPage />
      </PublicRoute>
    ),
  },

  {
    element: (
      <ProtectedRoute>
        <AppLayout />
      </ProtectedRoute>
    ),
    children: [
      {
        path: "/",
        element: <DocumentsPage />,
      },
      {
        path: "/documents",
        element: <DocumentsPage />,
      },
      {
        path: "/documents/:id/share",
        element: <SharingPage />,
      },
      {
        path: "/audit",
        element: <AuditLogPage />,
      },
    ],
  },

  {
    path: "*",
    element: <NotFoundPage />,
  },
]);

export function AppRoutes() {
  return <RouterProvider router={router} />;
}
