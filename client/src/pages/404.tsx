import { Link } from '@tanstack/react-router';

export default function NotFoundPage() {
  return (
    <div className="w-full h-full flex items-center justify-center flex-col">
      <h1 className="text-red-500 font-bold text-5xl">404</h1>
      <Link to="/" className="text-xl">
        Go back to home
      </Link>
    </div>
  );
}
