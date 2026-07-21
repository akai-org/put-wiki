# JWT Frontend Implementation - Quick Start

## TL;DR - What the Frontend Needs to Do

### 1. Extract JWT from Callback
```typescript
const response = await fetch('/api/auth/callback?oauth_token=X&oauth_verifier=Y');
const { token } = await response.json();
localStorage.setItem('authToken', token);
```

### 2. Add to All Requests
Update `client/src/lib/api.ts`:
```typescript
import axios from 'axios';

const agent = axios.create({
  baseURL: '/',
  withCredentials: true,
});

agent.interceptors.request.use((config) => {
  const token = localStorage.getItem('authToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

agent.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('authToken');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export { agent };
```

### 3. Test It
```bash
# Frontend should now automatically include JWT in all requests
# No additional code needed for individual API calls
```

---

## API Endpoints You Need to Call

| Endpoint | Method | Purpose | Auth |
|----------|--------|---------|------|
| `/api/auth/login` | GET | Start USOS OAuth flow | ❌ No |
| `/api/auth/callback` | GET | Receive JWT after OAuth | ❌ No |
| `/api/user/profile` | GET | Test protected endpoint | ✅ Yes (Bearer) |

---

## Response Format from Callback

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "hashedUsosId": "hashed_value_123"
}
```

---

## Error Handling

| HTTP Status | Meaning | What to Do |
|------------|---------|-----------|
| 200 | Success | Store token and proceed |
| 400 | Bad request | Retry OAuth flow |
| 401 | Token invalid/expired | Redirect to login |
| 502 | Backend error | Show error message |

---

## That's It! 🎉

The backend now:
- ✅ Generates JWT after USOS OAuth
- ✅ Returns it in callback response
- ✅ Validates it on protected endpoints
- ✅ Rejects requests without valid token

Your frontend just needs to store it and pass it in the `Authorization: Bearer {token}` header.

See `JWT_IMPLEMENTATION_GUIDE.md` for detailed information.
