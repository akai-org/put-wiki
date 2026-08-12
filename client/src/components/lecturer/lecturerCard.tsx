import { useState } from 'react';
import type { LecturerCard } from '@/schemas/lecturer/lecturerCard';

export default function LecturerCard({ title, name, photo_url }: LecturerCard) {
  const [open, setOpen] = useState(false);

  return (
    <div className="flex flex-row gap-2 p-2 pr-5 bg-gray-50 rounded-xl items-center">
      <button
        onClick={() => setOpen(true)}
        className="p-0 border-0 bg-transparent rounded-full overflow-hidden cursor-pointer"
        aria-label="Open lecturer photo"
      >
        <img
          src={`${photo_url}`}
          className="w-12 h-12 md:w-20 md:h-20 lg:w-28 lg:h-28 rounded-full object-cover"
          alt="lecturer face"
        />
      </button>

      <div className="flex flex-col">
        <p className="font-bold text-2xl md:text-4xl">{title}</p>
        <p className="font-bold text-xl md:text-3xl">{name}</p>
        <div>tagi specjalizacje</div>
      </div>

      {open && (
        <button
          className="fixed inset-0 z-50 flex items-center justify-center bg-black/60"
          onClick={() => setOpen(false)}
        >
          <div className="bg-white p-4 rounded-lg max-w-[90vw] max-h-[90vh]">
            <img
              src={`${photo_url}`}
              className="max-h-[80vh] max-w-[80vw] object-contain"
              alt="lecturer large"
            />
          </div>
        </button>
      )}
    </div>
  );
}
