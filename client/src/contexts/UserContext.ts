import { createContext } from 'react';

export interface UserContextValue {
  nickname: string;
}

export const UserContext = createContext<UserContextValue | null>(null);
