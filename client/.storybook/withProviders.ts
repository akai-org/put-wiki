import React from 'react';
import type { Decorator } from '@storybook/react-vite';
import { AuthProvider } from '../src/contexts/AuthProvider';
import { ThemeProvider } from '../src/contexts/ThemeProvider';

export function withProviders(options: {
  initialLoggedIn?: boolean;
  initialDark?: boolean;
}): Decorator {
  const decorator: Decorator = (Story) =>
    React.createElement(
      AuthProvider,
      { initialLoggedIn: options.initialLoggedIn },
      React.createElement(
        ThemeProvider,
        { initialDark: options.initialDark },
        React.createElement(Story)
      )
    );
  return decorator;
}
