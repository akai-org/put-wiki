import { createFileRoute } from '@tanstack/react-router';
import { Button } from '@/components/ui/Button';
import { useLogin } from '@/features/auth/api/useLogin';
export const Route = createFileRoute('/')({
  component: Home,
});

function Home() {
  const { mutate: login, isPending } = useLogin();

  return (
    <div className="mx-auto grid w-full max-w-7xl flex-1 grid-cols-1 items-center gap-10 px-15 py-10 md:grid-cols-2 lg:px-16">
      <div className="flex flex-col items-center gap-6 text-center md:items-start md:gap-7 md:text-left">
        <div className="flex w-full max-w-lg flex-col items-center gap-4 md:items-start md:gap-4.5">
          <h1 className="font-serif text-5xl leading-none font-normal tracking-normal md:text-7xl">
            Twoja uczelnia...
            <br />
            Twoi ludzie
          </h1>

          <p className="max-w-md text-sm leading-relaxed text-foreground/90 md:text-base">
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque vestibulum arcu a
            lorem dignissim, a aliquam ipsum finibus.
          </p>
        </div>
        <div>
          <Button
            className="h-11 w-fit cursor-pointer self-center rounded-xl bg-[hsla(216,50%,12%,1)] px-8 py-2.5 text-sm font-normal text-[hsla(0,0%,100%,1)] shadow-[0_12px_32px_rgba(15,23,42,0.35)] transition-all hover:bg-[hsla(216,50%,18%,1)] sm:h-12 sm:w-56 md:self-start xl:h-12.5 xl:w-64 xl:text-base"
            disabled={isPending}
            onClick={() => login()}
          >
            {isPending ? 'Logowanie...' : 'Zaloguj przez USOS'}
          </Button>
        </div>
      </div>
      <div />
    </div>
  );
}
