import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/')({
  component: Home,
});

function Home() {
  return (
    <div className="flex size-full items-center justify-center pt-24 lg:pt-30">
      <h1 className="text-5xl font-bold">PUT Wiki Home</h1>
    </div>
  );
}
