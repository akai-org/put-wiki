import type { Opinion } from '@/features/opinions/opinionSchema';

export default function OpinionLabel({ userName, content, rating }: Opinion) {
  return (
    <div className="">
      <h3>{userName}</h3>
      <p>{content}</p>
      <p>Ocena: {rating}</p>
    </div>
  );
}
