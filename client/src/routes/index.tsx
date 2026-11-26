import { createFileRoute } from '@tanstack/react-router';
import { Button } from '@/components/ui/Button';
export const Route = createFileRoute('/')({
  component: Home,
});

function Home() {
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
          <Button />
        </div>
      </div>
      <div />
    </div>
  );
}
