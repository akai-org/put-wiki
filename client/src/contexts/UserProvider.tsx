import { useState, type ReactNode } from 'react';
import { UserContext } from './UserContext';

const DEFAULT_NICKNAME = 'Anonim';

export function UserProvider({
  children,
  initialNickname = DEFAULT_NICKNAME,
}: {
  children: ReactNode;
  initialNickname?: string;
}) {
  const [nickname] = useState(initialNickname);

  return <UserContext value={{ nickname }}>{children}</UserContext>;
}
