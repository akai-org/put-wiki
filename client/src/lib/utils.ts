import { clsx, type ClassValue } from 'clsx';
import { twMerge } from 'tailwind-merge';

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

/*
Calculates the average from an array of numbers. 
Returns a string formatted to 2 decinaml places.
If the array is empty, undefined or null returns "0.00".
*/
export function calculateAverage(numbers: number[] | undefined | null): string {
  if (!numbers || numbers.length === 0) return '0.00';
  const sum = numbers.reduce((a, b) => a + b, 0);
  return (sum / numbers.length).toFixed(2);
}
