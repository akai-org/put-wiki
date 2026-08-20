import * as React from 'react';

import { Button } from '@/components/ui/Button';

type ToggleProps = React.ComponentPropsWithoutRef<typeof Button> & {
  onValue: string;
  offValue: string;
};

const Toggle = React.forwardRef<HTMLButtonElement, ToggleProps>(function Toggle(
  { onValue, offValue, onClick, ...props },
  ref
) {
  const [isClicked, setIsClicked] = React.useState(false);

  function handleClick(event: React.MouseEvent<HTMLButtonElement>) {
    setIsClicked((current) => !current);
    onClick?.(event);
  }

  return (
    <Button onClick={handleClick} ref={ref} {...props}>
      {isClicked ? onValue : offValue}
    </Button>
  );
});

Toggle.displayName = 'Toggle';

export default Toggle;
