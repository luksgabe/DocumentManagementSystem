import { Link, useNavigate } from "react-router-dom";

export default function Navbar() {
  const navigate = useNavigate();

  const logout = () => {
    localStorage.removeItem("authToken");
    navigate("/login");
  };

  return (
    <nav className="w-full bg-card border-b border-border py-4 shadow-sm">
      <div className="max-w-6xl mx-auto flex items-center justify-between px-6">
        <div className="flex items-center gap-6 text-sm font-medium">
          <Link to="/documents" className="hover:text-primary transition">
            Documents
          </Link>

          <Link to="/audit" className="hover:text-primary transition">
            Audit Log
          </Link>
        </div>

        <button
          onClick={logout}
          className="text-sm bg-primary text-primary-foreground px-4 py-2 rounded-md hover:bg-primary/90 transition"
        >
          Logout
        </button>
      </div>
    </nav>
  );
}
