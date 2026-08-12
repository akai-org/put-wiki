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
        <Settings className="h-5 w-5" />
      </div>
      <input type="text" placeholder={placeholder} className={inputClassName} />
    </div>
  );
}
