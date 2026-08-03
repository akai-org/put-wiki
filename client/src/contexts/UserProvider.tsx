import { useState, type ReactNode } from 'react';
import { UserContext } from './UserContext';

const MOCK_NICKNAME = 'Janek';

export function UserProvider({
  children,
  initialNickname = MOCK_NICKNAME,
}: {
  children: ReactNode;
  initialNickname?: string;
}) {
  const [nickname] = useState(initialNickname);

  return <UserContext value={{ nickname }}>{children}</UserContext>;
}
