import * as React from 'react';

import { Button } from '@/components/ui/button';

type ToggleProps = React.ComponentPropsWithoutRef<typeof Button> & {
  OnValue: string;
  OffValue: string;
};

const Toggle = React.forwardRef<HTMLButtonElement, ToggleProps>(function Toggle(
  { OnValue, OffValue, onClick, ...props },
  ref
) {
  const [isClicked, setIsClicked] = React.useState(false);

  function handleClick(event: React.MouseEvent<HTMLButtonElement>) {
    setIsClicked((current) => !current);
    onClick?.(event);
  }

  return (
    <Button ref={ref} onClick={handleClick} {...props}>
      {isClicked ? OnValue : OffValue}
    </Button>
  );
});

Toggle.displayName = 'Toggle';

export default Toggle;
