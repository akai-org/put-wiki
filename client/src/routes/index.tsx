import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/')({
  component: Home,
});

function Home() {
  return (
    <div className="flex size-full items-center justify-center">
      <h1 className="text-5xl font-bold">PUT Wiki Home</h1>
    </div>
  );
}
