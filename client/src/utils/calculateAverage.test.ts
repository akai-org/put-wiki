import { describe, expect, it } from 'vitest';
import { calculateAverage } from './calculateAverage';

describe('calculateAverage', () => {
  it('should return the correct formatted average for a valid array of numbers', () => {
    // given
    const numbers = [10, 20, 30];

    // when
    const result = calculateAverage(numbers);

    // then
    expect(result).toBe('20.00');
  });

  it('should round the average to two decimal places correctly', () => {
    // given
    const numbers = [1, 2, 4];

    // when
    const result = calculateAverage(numbers);

    // then
    expect(result).toBe('2.33');
  });

  it('should return 0.00 when given an empty array', () => {
    // given
    const numbers: number[] = [];

    // when
    const result = calculateAverage(numbers);

    // then
    expect(result).toBe('0.00');
  });

  it('should return 0.00 when input is null', () => {
    // given
    const numbers = null;

    // when
    const result = calculateAverage(numbers);

    // then
    expect(result).toBe('0.00');
  });

  it('should return 0.00 when input is undefined', () => {
    // given
    const numbers = undefined;

    // when
    const result = calculateAverage(numbers);

    // then
    expect(result).toBe('0.00');
  });

  it('should handle negative numbers correctly', () => {
    // given
    const numbers = [-10, 5, -5];

    // when
    const result = calculateAverage(numbers);

    // then
    expect(result).toBe('-3.33');
  });

  it('should handle floating point numbers', () => {
    // given
    const numbers = [1.5, 2.25, 3.75];

    // when
    const result = calculateAverage(numbers);

    // then
    expect(result).toBe('2.50');
  });
});
