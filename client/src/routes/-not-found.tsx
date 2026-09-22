import { useNavigate } from '@tanstack/react-router';
import notFoundImage from '@/assets/graphics/cats/sadCats.svg';

export default function GlobalNotFoundPage() {
  const navigate = useNavigate();

  return (
    <div className="flex size-full flex-col items-center justify-center p-6">
      <h1 className="text-6xl font-bold text-destructive">404</h1>
      <img
        src={notFoundImage}
        alt="Two sad cats beneath doodles; the cat on the left is wearing an AKAI T-shirt."
        className="w-1/3 h-1/3 p-2"
      />
      <button
        className="rounded-lg bg-secondary px-2 py-1 my-2 text-xl text-secondary-foreground transition-colors hover:bg-secondary/80 focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2"
        type="button"
        onClick={() => navigate({ to: '/' })}
      >
        Go back to home
      </button>
    </div>
  );
}
