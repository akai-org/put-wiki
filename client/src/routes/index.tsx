import { createFileRoute } from '@tanstack/react-router';

export const Route = createFileRoute('/')({
  component: Home,
});

function Home() {
  return (
    <div className="w-full h-full flex items-center justify-center">
      <h1 className="font-bold text-5xl">PUT Wiki Home</h1>
    </div>
  );
}
