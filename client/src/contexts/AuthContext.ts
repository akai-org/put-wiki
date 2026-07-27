import { createContext } from 'react';

export interface AuthContextValue {
  isLoggedIn: boolean;
  nickname: string;
  login: () => void;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextValue | null>(null);
