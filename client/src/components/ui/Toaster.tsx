import {
  CircleCheckIcon,
  InfoIcon,
  Loader2Icon,
  OctagonXIcon,
  TriangleAlertIcon,
} from 'lucide-react';
import { useTheme } from '@/hooks/useTheme';
import { Toaster as Sonner, type ToasterProps } from 'sonner';

function Toaster({ ...props }: ToasterProps) {
  const { isDark } = useTheme();

  return (
    <Sonner
      icons={{
        success: <CircleCheckIcon className="size-4" />,
        info: <InfoIcon className="size-4" />,
        warning: <TriangleAlertIcon className="size-4" />,
        error: <OctagonXIcon className="size-4" />,
        loading: <Loader2Icon className="size-4 animate-spin" />,
      }}
      richColors
      style={
        {
          '--normal-bg': 'var(--popover)',
          '--normal-text': 'var(--popover-foreground)',
          '--normal-border': 'var(--border)',
          '--success-bg': 'hsl(142 76% 36%)',
          '--success-border': 'hsl(142 72% 29%)',
          '--success-text': 'hsl(0 0% 100%)',
          '--info-bg': 'hsl(221 83% 53%)',
          '--info-border': 'hsl(221 83% 45%)',
          '--info-text': 'hsl(0 0% 100%)',
          '--warning-bg': 'hsl(45 93% 47%)',
          '--warning-border': 'hsl(38 92% 40%)',
          '--warning-text': 'hsl(24 100% 10%)',
          '--error-bg': 'hsl(0 84% 60%)',
          '--error-border': 'hsl(0 74% 50%)',
          '--error-text': 'hsl(0 0% 100%)',
          '--border-radius': 'var(--radius)',
        } as React.CSSProperties
      }
      theme={isDark ? 'dark' : 'light'}
      toastOptions={{
        duration: 4000, // 4 seconds
        classNames: {
          toast: 'border shadow-sm',
        },
      }}
      {...props}
    />
  );
}
export { Toaster };
