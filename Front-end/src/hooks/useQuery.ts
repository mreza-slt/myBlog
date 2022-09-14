import { useLocation } from "react-router-dom";

export function useQuery(): URLSearchParams {
  const location = useLocation();

  return new URLSearchParams(location.search);
}
