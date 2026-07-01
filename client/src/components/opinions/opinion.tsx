interface Opinion {
  author: string;
  content: string;
  rating: number;
}
export default function Opinion({ author, content, rating }: Opinion) {
  return (
    <div className="opinion">
      <h3>{author}</h3>
      <p>{content}</p>
      <p>Ocena: {rating}</p>
    </div>
  );
}
