import { Settings } from 'lucide-react';

export function SearchBar({
  containerClassName,
  iconWrapperClassName,
  inputClassName,
  placeholder = 'Wyszukaj',
}: {
  containerClassName: string;
  iconWrapperClassName: string;
  inputClassName: string;
  placeholder?: string;
}) {
  return (
    <div className={containerClassName}>
      <div className={iconWrapperClassName}>
        <Settings className="size-5" />
      </div>
      <input className={inputClassName} placeholder={placeholder} type="text" />
    </div>
  );
}
