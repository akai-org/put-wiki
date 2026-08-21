import { Link } from '@tanstack/react-router';

export default function NotFoundPage() {
  return (
    <div className="flex size-full flex-col items-center justify-center">
      <h1 className="text-5xl font-bold text-red-500">404</h1>
      <Link className="text-xl" to="/">
        Go back to home
      </Link>
    </div>
  );
}
