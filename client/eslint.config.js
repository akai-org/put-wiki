// For more info, see https://github.com/storybookjs/eslint-plugin-storybook#configuration-flat-config-format
import storybook from 'eslint-plugin-storybook';

import js from '@eslint/js';
import globals from 'globals';
import react from 'eslint-plugin-react';
import reactHooks from 'eslint-plugin-react-hooks';
import reactRefresh from 'eslint-plugin-react-refresh';
import tseslint from 'typescript-eslint';
import jsxA11y from 'eslint-plugin-jsx-a11y';
import eslintConfigPrettier from 'eslint-config-prettier/flat';
import query from '@tanstack/eslint-plugin-query';
import importPlugin from 'eslint-plugin-import';
import pluginRouter from '@tanstack/eslint-plugin-router';
import eslintPluginTailwindcss from 'eslint-plugin-tailwindcss';
import { defineConfig, globalIgnores } from 'eslint/config';

export default defineConfig([
  globalIgnores(['dist', 'coverage', '**/*.gen.ts', 'public']),
  {
    files: ['**/*.{ts,tsx}'],
    extends: [
      js.configs.recommended,
      tseslint.configs.recommended,
      react.configs.flat.recommended,
      reactHooks.configs.flat.recommended,
      reactRefresh.configs.vite,
      jsxA11y.flatConfigs.recommended,
      query.configs['flat/recommended'],
      storybook.configs['flat/recommended'],
      importPlugin.flatConfigs.recommended,
      ...pluginRouter.configs['flat/recommended'],
      eslintPluginTailwindcss.configs['flat/recommended'] ||
        eslintPluginTailwindcss.configs.recommended,
    ],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
    },
    plugins: {
      'react-hooks': reactHooks,
    },
    settings: {
      react: {
        version: 'detect',
      },
      'import/resolver': {
        typescript: {
          project: './tsconfig.json',
        },
      },
      tailwindcss: {
        cssConfigPath: './src/styles/tailwind.css',
      },
    },
    rules: {
      'react/react-in-jsx-scope': 'off',
      'react/function-component-definition': [
        'error',
        {
          namedComponents: 'function-declaration',
        },
      ],
      'react/prefer-stateless-function': 'error',
      'react/button-has-type': 'error',
      'react/no-unused-prop-types': 'error',
      'react/jsx-pascal-case': 'error',
      'react/jsx-no-script-url': 'error',
      'react/no-children-prop': 'error',
      'react/no-danger': 'error',
      'react/no-danger-with-children': 'error',
      'react/no-unstable-nested-components': ['error', { allowAsProps: true }],
      'react/jsx-fragments': 'error',
      'react/destructuring-assignment': ['error', 'always', { destructureInSignature: 'always' }],
      'react/jsx-no-leaked-render': ['error', { validStrategies: ['ternary'] }],
      'react/jsx-max-depth': ['error', { max: 5 }],
      'react/jsx-key': [
        'error',
        {
          checkFragmentShorthand: true,
          checkKeyMustBeforeSpread: true,
          warnOnDuplicates: true,
        },
      ],
      'react/jsx-no-useless-fragment': 'warn',
      'react/jsx-curly-brace-presence': 'warn',
      'react/no-typos': 'warn',
      'react/display-name': 'warn',
      'react/self-closing-comp': 'warn',
      'react/jsx-sort-props': 'warn',
      'react/jsx-one-expression-per-line': 'off',
      'react/prop-types': 'off',
      '@typescript-eslint/naming-convention': [
        'error',
        {
          selector: 'default',
          format: ['strictCamelCase'],
          leadingUnderscore: 'allow',
        },
        {
          selector: 'import',
          format: ['strictCamelCase', 'StrictPascalCase'],
          leadingUnderscore: 'allow',
        },
        {
          selector: 'function',
          format: ['strictCamelCase', 'StrictPascalCase'],
          leadingUnderscore: 'forbid',
        },
        {
          selector: 'enumMember',
          format: ['UPPER_CASE'],
          leadingUnderscore: 'forbid',
        },
        {
          selector: 'variable',
          format: ['strictCamelCase', 'StrictPascalCase', 'UPPER_CASE'],
          leadingUnderscore: 'allow',
        },
        {
          selector: 'parameter',
          format: ['strictCamelCase', 'StrictPascalCase'],
          leadingUnderscore: 'allow',
        },
        {
          selector: 'property',
          format: null,
          leadingUnderscore: 'allow',
        },
        {
          selector: 'typeLike',
          format: ['StrictPascalCase'],
        },
      ],
      'no-unused-vars': 'off',
      '@typescript-eslint/no-unused-vars': [
        'error',
        {
          args: 'all',
          argsIgnorePattern: '^_',
          caughtErrors: 'all',
          caughtErrorsIgnorePattern: '^_',
          destructuredArrayIgnorePattern: '^_',
          varsIgnorePattern: '^_',
          ignoreRestSiblings: true,
        },
      ],
      'tailwindcss/no-arbitrary-value': 'warn',
      'tailwindcss/no-custom-classname': ['warn', { whitelist: ['c\\-*'] }],
      'tailwindcss/no-contradicting-classname': 'error',
      'import/no-restricted-paths': [
        'error',
        {
          zones: [
            // disables cross-feature imports:
            // e.g. src/features/courses should not import from src/features/degree-courses, etc.
            // eg. src/features/discussions should not import from src/features/comments, etc.
            {
              target: './src/features/auth',
              from: './src/features',
              except: ['./auth'],
            },
            {
              target: './src/features/courses',
              from: './src/features',
              except: ['./courses'],
            },
            {
              target: './src/features/degree-courses',
              from: './src/features',
              except: ['./degree-courses'],
            },
            {
              target: `./src/features/lectures`,
              from: './src/features',
              except: ['./lectures'],
            },
          ],
        },
      ],
    },
  },
  eslintConfigPrettier, //should be last
]);
