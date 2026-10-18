import { CalendarDays, CircleCheckBig, CircleQuestionMark } from 'lucide-react';
import { Tooltip, TooltipContent, TooltipTrigger } from '@/components/ui/Tooltip';
import type { UserPublicProfileStatusProps } from '../schemas/userPublicProfileStatusSchema';

export function UserPublicProfileStatus({ joinedDate, userStatus }: UserPublicProfileStatusProps) {
  return (
    <div className="flex justify-center gap-8">
      <p className="flex items-center gap-2">
        <CalendarDays aria-hidden="true" className="size-4" />
        <span>Dołączono</span>
        <span className="whitespace-nowrap">{joinedDate}</span>
      </p>
      <p className="relative flex items-center gap-2">
        <CircleCheckBig aria-hidden="true" className="size-4" />
        Status: {userStatus}
        <span className="absolute top-1/2 left-full ml-1 -translate-y-1/2">
          <Tooltip>
            <TooltipTrigger>
              <CircleQuestionMark
                aria-label="Informacje o statusie użytkownika"
                className="size-4"
                opacity={0.4}
              />
            </TooltipTrigger>
            <TooltipContent>
              <p>Informacje o statusie użytkownika</p>
            </TooltipContent>
          </Tooltip>
        </span>
      </p>
    </div>
  );
}
