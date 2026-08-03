import { afterEach, beforeEach, describe, expect, it } from 'vitest';
import { act, renderHook } from '@testing-library/react';
import { useTheme } from './useTheme';
import { ThemeProvider } from '@/contexts/ThemeProvider';
import type { Theme } from '@/contexts/ThemeContext';

function wrapper({ initialTheme, children }: { initialTheme?: Theme; children: React.ReactNode }) {
  return <ThemeProvider initialTheme={initialTheme}>{children}</ThemeProvider>;
}

describe('useTheme', () => {
  beforeEach(() => {
    localStorage.clear();
    document.documentElement.classList.remove('dark');
  });

  afterEach(() => {
    localStorage.clear();
    document.documentElement.classList.remove('dark');
  });

  it('throws when used outside of a ThemeProvider', () => {
    expect(() => renderHook(() => useTheme())).toThrowError(
      'useTheme must be used within a ThemeProvider'
    );
  });

  it('initializes from the initialTheme prop and applies the dark class', () => {
    const { result } = renderHook(() => useTheme(), {
      wrapper: (props) => wrapper({ initialTheme: 'dark', ...props }),
    });

    expect(result.current.theme).toBe('dark');
    expect(result.current.isDark).toBe(true);
    expect(result.current.isLight).toBe(false);
    expect(document.documentElement.classList.contains('dark')).toBe(true);
    expect(localStorage.getItem('theme')).toBe('dark');
  });

  it('initializes from localStorage when no override is given', () => {
    localStorage.setItem('theme', 'dark');

    const { result } = renderHook(() => useTheme(), {
      wrapper: (props) => wrapper({ ...props }),
    });

    expect(result.current.theme).toBe('dark');
    expect(document.documentElement.classList.contains('dark')).toBe(true);
  });

  it('updates the dark class and localStorage when setTheme is called', () => {
    const { result } = renderHook(() => useTheme(), {
      wrapper: (props) => wrapper({ initialTheme: 'light', ...props }),
    });

    act(() => {
      result.current.setTheme('dark');
    });

    expect(result.current.theme).toBe('dark');
    expect(result.current.isDark).toBe(true);
    expect(document.documentElement.classList.contains('dark')).toBe(true);
    expect(localStorage.getItem('theme')).toBe('dark');
  });
});
