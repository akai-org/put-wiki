// Accepts only same-origin relative paths to prevent open redirects
export function toSafeRedirectPath(path: string | undefined, fallback = '/'): string {
  if (!path?.startsWith('/') || path.startsWith('//') || path.startsWith('/\\')) {
    return fallback;
  }

  return path;
}
