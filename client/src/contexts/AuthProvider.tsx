import { useState, type ReactNode } from 'react';
import { AuthContext } from './AuthContext';

const MOCK_NICKNAME = 'Janek';

export function AuthProvider({
  children,
  initialLoggedIn = false,
  initialNickname = MOCK_NICKNAME,
}: {
  children: ReactNode;
  initialLoggedIn?: boolean;
  initialNickname?: string;
}) {
  const [isLoggedIn, setIsLoggedIn] = useState(initialLoggedIn);
  const [nickname] = useState(initialNickname);

  return (
    <AuthContext.Provider
      value={{
        isLoggedIn,
        nickname,
        login: () => setIsLoggedIn(true),
        logout: () => setIsLoggedIn(false),
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
