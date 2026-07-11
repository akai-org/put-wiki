import { toast } from 'sonner';

export function useToast() {
  function ShowErrorToast(message: string) {
    toast.error('Error: ' + message, {
      duration: 5000,
      position: 'top-center',
      className: 'bg-red-500 text-red-900',
    });
  }

  function ShowMessageToast(message: string) {
    toast(message, {
      duration: 4000,
      position: 'top-center',
      className: 'bg-yellow-500 text-black',
    });
  }

  function ShowSuccessToast(message: string) {
    toast.success(message, {
      duration: 4000,
      position: 'top-center',
      className: 'bg-green-500 text-black',
    });
  }

  return { ShowErrorToast, ShowMessageToast, ShowSuccessToast };
}
