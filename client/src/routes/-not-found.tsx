import { useNavigate } from '@tanstack/react-router';
import notFoundImage from '@/assets/graphics/cats/sadCats.svg';
import { Button } from '@/components/ui/Button';

export default function GlobalNotFoundPage() {
  const navigate = useNavigate();

  return (
    <div className="flex size-full flex-col items-center justify-center p-6">
      <div className="grid place-items-center">
        <h1 className="col-1 row-1 text-[20vmax] leading-none font-bold text-accent">404</h1>
        <img
          alt="Two sad cats beneath doodles; the cat on the left is wearing an AKAI T-shirt."
          className="z-10 col-1 row-1 mt-20 size-[15vmax] w-full p-2"
          src={notFoundImage}
        />
      </div>
      <p>Ta strona ma dziś dziewięć żyć mniej.</p>
      <p>Nie udało nam się jej odnaleźć.</p>
      <Button
        className="mt-4 rounded-4xl p-6"
        onClick={() => navigate({ to: '/' })}
        variant="secondary"
      >
        Wróć na stronę główną
      </Button>
    </div>
  );
}
