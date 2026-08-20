import type { Opinion } from '@/schemas/opinion';

export default function OpinionLabel({ userName, content, rating }: Opinion) {
  return (
    <div className="opinion">
      <h3>{userName}</h3>
      <p>{content}</p>
      <p>Ocena: {rating}</p>
    </div>
  );
}
