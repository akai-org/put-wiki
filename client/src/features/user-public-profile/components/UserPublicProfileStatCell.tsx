import type { UserPublicProfileStatCellProps } from '../schemas/userPublicProfileStatCellSchema';

export function UserPublicProfileStatCell({
  className,
  label,
  value,
  valueClassName,
}: UserPublicProfileStatCellProps) {
  return (
    <div className={className}>
      <p>{label}</p>
      <p className={valueClassName}>{value}</p>
    </div>
  );
}
