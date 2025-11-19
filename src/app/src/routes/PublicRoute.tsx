import { Navigate } from "react-router-dom";

type Props = {
  children: React.ReactNode;
};

export function PublicRoute({ children }: Props) {
  const isAuthenticated = !!localStorage.getItem("token");

  if (isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  return <>{children}</>;
}
