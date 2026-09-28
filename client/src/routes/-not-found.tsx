import { useNavigate } from '@tanstack/react-router';
import notFoundImage from '@/assets/graphics/cats/sadCats.svg';
import { Button } from '@/components/ui/Button';

export default function GlobalNotFoundPage() {
  const navigate = useNavigate();

  return (
    <div className="flex size-full flex-col items-center justify-center p-6">
      <h1 className="text-6xl font-bold text-destructive">404</h1>
      <img
        alt="Two sad cats beneath doodles; the cat on the left is wearing an AKAI T-shirt."
        className="size-1/3 p-2"
        src={notFoundImage}
      />

      <Button onClick={() => navigate({ to: '/' })} variant="secondary">
        Go back to home
      </Button>
    </div>
  );
}
