import { AuthContext } from '@/contexts/AuthContext';
import { useContext, useState } from 'react';

export default function LoginPage() {
  const auth = useContext(AuthContext);

  const [keepLogin, setKeeplogin] = useState(false);

  const handleSendLogin = async () => {
    // This will fail for
    const response = await fetch(
      '/api/auth/callback?oauth_token={token}&oauth_verifier={verifier}'
    );
    const data = await response.json();
    const jwtToken = data.token;
    if (keepLogin) {
      localStorage.setItem('authToken', jwtToken);
    } else {
      sessionStorage.setItem('authToken', jwtToken);
    }
    auth.setSession({
      userId: data.userId,
    });
  };

  return (
    <>
      <div className="w-full h-full flex flex-col items-center justify-center">
        <h1>Login Page Stub</h1>
        <button onClick={handleSendLogin}>Send login request</button>
        <input
          type="checkbox"
          onChange={(e) => {
            setKeeplogin(e.target.checked);
          }}
        />
      </div>
    </>
  );
}
