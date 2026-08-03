import { describe, expect, it } from 'vitest';
import { act, renderHook } from '@testing-library/react';
import { useAuth } from './useAuth';
import { AuthProvider } from '@/contexts/AuthProvider';

function wrapper({
  initialLoggedIn,
  children,
}: {
  initialLoggedIn?: boolean;
  children: React.ReactNode;
}) {
  return <AuthProvider initialLoggedIn={initialLoggedIn}>{children}</AuthProvider>;
}

describe('useAuth', () => {
  it('throws when used outside of an AuthProvider', () => {
    expect(() => renderHook(() => useAuth())).toThrowError(
      'useAuth must be used within an AuthProvider'
    );
  });

  it('defaults to logged out', () => {
    const { result } = renderHook(() => useAuth(), {
      wrapper: (props) => wrapper({ ...props }),
    });

    expect(result.current.isLoggedIn).toBe(false);
  });

  it('respects initialLoggedIn', () => {
    const { result } = renderHook(() => useAuth(), {
      wrapper: (props) => wrapper({ initialLoggedIn: true, ...props }),
    });

    expect(result.current.isLoggedIn).toBe(true);
  });

  it('login() sets isLoggedIn to true', () => {
    const { result } = renderHook(() => useAuth(), {
      wrapper: (props) => wrapper({ ...props }),
    });

    act(() => result.current.login());
    expect(result.current.isLoggedIn).toBe(true);
  });

  it('logout() sets isLoggedIn to false and is idempotent', () => {
    const { result } = renderHook(() => useAuth(), {
      wrapper: (props) => wrapper({ initialLoggedIn: true, ...props }),
    });

    act(() => result.current.logout());
    expect(result.current.isLoggedIn).toBe(false);

    act(() => result.current.logout());
    expect(result.current.isLoggedIn).toBe(false);
  });
});
