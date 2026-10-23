import { CircleQuestionMark } from 'lucide-react';
import { Tooltip, TooltipContent, TooltipTrigger } from '@/components/ui/Tooltip';
import type { UserPublicProfileStatsProps } from '../schemas/userPublicProfileStatsSchema';
import { UserPublicProfileStatCell } from './UserPublicProfileStatCell';

export function UserPublicProfileStats({
  karma,
  opinions,
  reactions,
}: UserPublicProfileStatsProps) {
  return (
    <div className="mt-4 grid w-full grid-cols-1 gap-y-8 md:grid-cols-3 md:gap-y-0">
      <UserPublicProfileStatCell
        className="order-2 flex flex-col items-center gap-4 text-center text-3xl md:order-0"
        label="Komentarze"
        value={opinions}
        valueClassName="text-1xl font-bold"
      />

      <UserPublicProfileStatCell
        className="order-first flex flex-col items-center gap-3 text-center text-4xl md:order-0"
        label={
          <span className="relative">
            Karma
            <span className="absolute top-1/2 left-full ml-1 -translate-y-1/2">
              <Tooltip>
                <TooltipTrigger>
                  <CircleQuestionMark
                    aria-label="Informacje o karmie"
                    className="size-5"
                    opacity={0.4}
                  />
                </TooltipTrigger>
                <TooltipContent>
                  <p>Informacje o karmie</p>
                </TooltipContent>
              </Tooltip>
            </span>
          </span>
        }
        value={karma}
        valueClassName="text-4xl font-bold"
      />

      <UserPublicProfileStatCell
        className="order-3 flex flex-col items-center gap-2 text-center text-3xl md:order-0"
        label="Reakcje"
        value={reactions}
        valueClassName="font-bold"
      />
    </div>
  );
}
