import { type IAuthSession } from '@/lib/types';
import { createContext, useState, type ReactNode } from 'react';

const AuthContext = createContext<{
  session: IAuthSession;
  setSession: (authSession: IAuthSession) => void;
}>({ session: { userId: null }, setSession: () => {} });

function AuthProvider({ children }: { children: ReactNode }) {
  const [session, setSession] = useState<IAuthSession>({ userId: null });
  return <AuthContext value={{ session, setSession }}>{children}</AuthContext>;
}

export { AuthContext };
export default AuthProvider;
