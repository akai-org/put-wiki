import { Button } from '@/components/ui/button.tsx';
import { useToast } from '@/hooks/useToast';

export default function ToasterPage() {
  const { ShowErrorToast, ShowSuccessToast, ShowMessageToast, ShowWarningToast } = useToast();

  return (
    <div className="flex flex-wrap gap-4 p-6">
      <Button onClick={() => ShowErrorToast('This is an error message')}>Show Error Toast</Button>
      <Button onClick={() => ShowSuccessToast('This is a success message')}>
        Show Success Toast
      </Button>
      <Button onClick={() => ShowMessageToast('This is a message')}>Show Message Toast</Button>
      <Button onClick={() => ShowWarningToast('This is a warning message')}>
        Show Warning Toast
      </Button>
    </div>
  );
}
