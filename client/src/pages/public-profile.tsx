import { useParams } from '@tanstack/react-router';

export default function PublicProfile() {
  const { userid } = useParams({ from: '/user/$userid' });
  return (
    <div>
      <h1>I am PublicProfile you are're viewing "{userid}"'s profile</h1>
    </div>
  );
}
