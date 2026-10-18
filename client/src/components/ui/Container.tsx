import { ComponentProps } from 'react';

type ContainerProps = ComponentProps<'div'>;

export function Container({ children, className = '', ...props }: ContainerProps) {
  return (
    <div {...props} className={`max-w-[1440px] mx-auto w-full ${className}`.trim()}>
      {children}
    </div>
  );
}
