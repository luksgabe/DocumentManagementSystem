import { Navigate } from "react-router-dom";

type Props = {
  children: React.ReactNode;
};

export function ProtectedRoute({ children }: Props) {
  const isAuthenticated = !!localStorage.getItem("authToken");

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
}
