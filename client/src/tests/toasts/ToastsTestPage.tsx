import { Button } from '@/components/ui/Button';
import { showToast } from '@/utils/showToasts';

export default function ToasterPage() {
  const { showErrorToast, showSuccessToast, showMessageToast, showWarningToast } = showToast();

  return (
    <div className="flex flex-wrap gap-4 p-6">
      <Button onClick={() => showErrorToast('This is an error message')}>Show Error Toast</Button>
      <Button onClick={() => showSuccessToast('This is a success message')}>
        Show Success Toast
      </Button>
      <Button onClick={() => showMessageToast('This is a message')}>Show Message Toast</Button>
      <Button onClick={() => showWarningToast('This is a warning message')}>
        Show Warning Toast
      </Button>
    </div>
  );
}
