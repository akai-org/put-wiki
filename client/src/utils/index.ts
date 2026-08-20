import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';
import { calculateAverage } from '@/utils/calculateAverage';

function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export { cn, calculateAverage };
