import { toast } from 'sonner';

export function useToast() {
  function ShowErrorToast(message: string) {
    toast.error('Error: ' + message, { position: 'top-center' });
  }

  function ShowMessageToast(message: string) {
    toast(message, { position: 'top-center' });
  }

  function ShowSuccessToast(message: string) {
    toast.success(message, { position: 'top-center' });
  }

  function ShowWarningToast(message: string) {
    toast.warning('Warning: ' + message, { position: 'top-center' });
  }

  return { ShowErrorToast, ShowMessageToast, ShowSuccessToast, ShowWarningToast };
}
