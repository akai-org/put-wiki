import { toast } from 'sonner';

export function useToast() {
  function showErrorToast(message: string) {
    toast.error('Error: ' + message, {});
  }

  function showMessageToast(message: string) {
    toast(message, {});
  }

  function showSuccessToast(message: string) {
    toast.success(message, {});
  }

  function showWarningToast(message: string) {
    toast.warning('Warning: ' + message, {});
  }

  return { showErrorToast, showMessageToast, showSuccessToast, showWarningToast };
}
