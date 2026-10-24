import { type ComponentProps } from 'react';
import { cn } from '@/utils';

type ContainerProps = ComponentProps<'div'>;

export function Container({ children, className }: ContainerProps) {
  return <div className={cn(`mx-auto w-full max-w-360 px-24`, className)}>{children}</div>;
}
