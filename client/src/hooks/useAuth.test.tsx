import { describe, expect, it } from 'vitest';
import { act, renderHook } from '@testing-library/react';
import { useAuth } from './useAuth';
import { AuthProvider } from '@/contexts/AuthProvider';

function wrapper({
  initialLoggedIn,
  initialNickname,
  children,
}: {
  initialLoggedIn?: boolean;
  initialNickname?: string;
  children: React.ReactNode;
}) {
  return (
    <AuthProvider initialLoggedIn={initialLoggedIn} initialNickname={initialNickname}>
      {children}
    </AuthProvider>
  );
}

describe('useAuth', () => {
  it('throws when used outside of an AuthProvider', () => {
    expect(() => renderHook(() => useAuth())).toThrowError(
      'useAuth must be used within an AuthProvider'
    );
  });

  it('defaults to logged out with the mock nickname', () => {
    const { result } = renderHook(() => useAuth(), {
      wrapper: (props) => wrapper({ ...props }),
    });

    expect(result.current.isLoggedIn).toBe(false);
    expect(result.current.nickname).toBe('Janek');
  });

  it('respects initialLoggedIn and initialNickname', () => {
    const { result } = renderHook(() => useAuth(), {
      wrapper: (props) => wrapper({ initialLoggedIn: true, initialNickname: 'Kasia', ...props }),
    });

    expect(result.current.isLoggedIn).toBe(true);
    expect(result.current.nickname).toBe('Kasia');
  });

  it('login() and logout() toggle isLoggedIn', () => {
    const { result } = renderHook(() => useAuth(), {
      wrapper: (props) => wrapper({ ...props }),
    });

    act(() => result.current.login());
    expect(result.current.isLoggedIn).toBe(true);

    act(() => result.current.logout());
    expect(result.current.isLoggedIn).toBe(false);
  });
});
