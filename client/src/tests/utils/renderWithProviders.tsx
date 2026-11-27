import type { ReactElement } from 'react';
import { render } from '@testing-library/react';
import { AppProviders, type AppProvidersOptions } from './AppProviders';

export function renderWithProviders(ui: ReactElement, options: AppProvidersOptions = {}) {
  return render(<AppProviders {...options}>{ui}</AppProviders>);
}
