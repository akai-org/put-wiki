import { useState, type ReactNode } from 'react';
import { AuthContext } from './AuthContext';

export function AuthProvider({
  children,
  initialLoggedIn = false,
}: {
  children: ReactNode;
  initialLoggedIn?: boolean;
}) {
  const [isLoggedIn, setIsLoggedIn] = useState(initialLoggedIn);

  return (
    <AuthContext
      value={{
        isLoggedIn,
        login: () => setIsLoggedIn(true),
        logout: () => setIsLoggedIn(false),
      }}
    >
      {children}
    </AuthContext>
  );
}
