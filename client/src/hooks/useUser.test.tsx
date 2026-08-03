import { describe, expect, it } from 'vitest';
import { renderHook } from '@testing-library/react';
import { useUser } from './useUser';
import { UserProvider } from '@/contexts/UserProvider';

function wrapper({
  initialNickname,
  children,
}: {
  initialNickname?: string;
  children: React.ReactNode;
}) {
  return <UserProvider initialNickname={initialNickname}>{children}</UserProvider>;
}

describe('useUser', () => {
  it('throws when used outside of a UserProvider', () => {
    expect(() => renderHook(() => useUser())).toThrowError(
      'useUser must be used within a UserProvider'
    );
  });

  it('defaults to the mock nickname', () => {
    const { result } = renderHook(() => useUser(), {
      wrapper: (props) => wrapper({ ...props }),
    });

    expect(result.current.nickname).toBe('Janek');
  });

  it('respects the initialNickname override', () => {
    const { result } = renderHook(() => useUser(), {
      wrapper: (props) => wrapper({ initialNickname: 'Kasia', ...props }),
    });

    expect(result.current.nickname).toBe('Kasia');
  });
});
