export function calculateAverage(numbers: number[] | undefined | null): string {
  if (!numbers || numbers.length === 0) return '0.00';
  const sum = numbers.reduce((a, b) => a + b, 0);
  return (sum / numbers.length).toFixed(2);
}
