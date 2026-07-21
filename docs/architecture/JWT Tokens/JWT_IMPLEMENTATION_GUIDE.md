# JWT Implementation Summary - Backend ✅ COMPLETE

## Overview
JWT authentication has been fully implemented in the backend. After successful USOS OAuth authentication and user provisioning, the system generates a signed JWT token that the frontend can use for subsequent API requests.

## Backend Implementation Details

### 1. **JWT Architecture**
- **Location**: Infrastructure layer (`Infrastructure.Auth.JwtService`)
- **Service Interface**: `IJwtService` (Application layer)
- **Algorithm**: HMAC-SHA256
- **Payload**: Contains only user ID (internal, anonymized) + standard JWT claims (iat, exp, iss, aud)

### 2. **New Files Created**
- `Infrastructure/Auth/Configuration/JwtSettings.cs` - Configuration class
- `Infrastructure/Auth/JwtService.cs` - JWT generation and validation service
- `Application/Auth/IJwtService.cs` - Service interface
- `Application/Features/Users/Commands/ProvisionUser/AuthTokenDto.cs` - Response DTO with JWT token

### 3. **Modified Files**
- `Presentation/Program.cs` - Added JWT Bearer authentication scheme configuration
- `Presentation/Controllers/AuthController.cs` - Updated callback endpoint to return JWT
- `Presentation/Controllers/UserController.cs` - Added protected endpoint example
- `Application/Features/Users/Commands/ProvisionUser/ProvisionUserUseCase.cs` - Now generates JWT after provisioning
- `Infrastructure/Extensions/InfrastructureConfiguration.cs` - Registered JWT service and configuration
- `Presentation/appsettings.json` - Added JWT configuration
- `server/.env.example` - Added JWT secret configuration
- `.csproj` files - Added JWT NuGet dependencies

### 4. **Configuration (appsettings.json)**
```json
"Jwt": {
  "Issuer": "PutWiki",
  "Audience": "PutWikiClient",
  "ExpirationMinutes": 1440
}
```

### 5. **Environment Variable (`.env`)**
```
Jwt__Secret=your-secret-key-min-32-bytes-or-will-be-hashed-to-256-bits
```

## API Endpoints

### 1. **OAuth Callback Endpoint** (Existing, Enhanced)
```
GET /api/auth/callback?oauth_token={token}&oauth_verifier={verifier}
```
**Response (200 OK)**:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "hashedUsosId": "hashed_usos_id_123"
}
```

**Error Responses**:
- `400 Bad Request` - Invalid OAuth parameters
- `401 Unauthorized` - USOS authentication failed
- `502 Bad Gateway` - External service error

### 2. **Protected Endpoint** (Test/Example)
```
GET /api/user/profile
Authorization: Bearer {JWT_TOKEN}
```
**Response (200 OK)**:
```json
{
  "userId": "550e8400-e29b-41d4-a716-446655440000",
  "isAuthenticated": true,
  "authenticationType": "Bearer"
}
```

**Error Responses**:
- `401 Unauthorized` - Missing or invalid token

## JWT Token Specifications

### Token Claims
```json
{
  "iat": 1688169600,              // Issued at
  "exp": 1688256000,              // Expires at (24 hours default)
  "iss": "PutWiki",               // Issuer
  "aud": "PutWikiClient",         // Audience
  "nameid": "user-uuid",          // NameIdentifier claim with internal user ID
  "sub": "user-uuid"              // Subject claim with internal user ID
}
```

### Token Validation
- ✅ Signature validation (HMAC-SHA256)
- ✅ Issuer validation
- ✅ Audience validation
- ✅ Expiration time validation
- ✅ Issued-at validation

## Acceptance Criteria ✅

- ✅ **JWT generated immediately after successful user provisioning** in OAuth callback flow
- ✅ **Lightweight payload** contains only internal user ID (no PII, fully anonymized)
- ✅ **Callback endpoint returns JWT** in response body
- ✅ **Backend exposes protected endpoint** with `[Authorize]` attribute (`/api/user/profile`)
- ✅ **Validates Bearer tokens** from `Authorization: Bearer {token}` header
- ✅ **Appropriate HTTP codes and error messages** returned:
  - `401 Unauthorized` for missing/invalid/expired tokens
  - `400 Bad Request` for malformed requests
  - `502 Bad Gateway` for external service errors

---

## Frontend Implementation Steps ⬇️

### Step 1: Extract JWT from OAuth Callback Response
After the user redirects from USOS and hits the callback endpoint:

```typescript
// When OAuth callback completes (redirected to /api/auth/callback)
const response = await fetch('/api/auth/callback?oauth_token={token}&oauth_verifier={verifier}');
const data = await response.json();
const jwtToken = data.token; // Extract the JWT token
```

### Step 2: Store JWT Token
Store the token in a secure location (choose one):

**Option A: LocalStorage** (Simple, suitable for SPAs)
```typescript
localStorage.setItem('authToken', jwtToken);
```

**Option B: SessionStorage** (Cleared on browser close)
```typescript
sessionStorage.setItem('authToken', jwtToken);
```

**Option C: Memory + HTTPOnly Cookie** (Most secure, requires backend support)
- Store in memory for current session
- Backend sets HTTPOnly, Secure cookie (recommended for production)

### Step 3: Add JWT to API Requests
Update your API client (you're using axios in `client/src/lib/api.ts`):

```typescript
// client/src/lib/api.ts
import axios from 'axios';

const agent = axios.create({
  baseURL: '/',
  withCredentials: true,
});

// Add request interceptor to inject JWT token
agent.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('authToken'); // or sessionStorage
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Add response interceptor to handle 401 responses
agent.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Token expired or invalid
      localStorage.removeItem('authToken'); // Clear token
      // Redirect to login or show re-authentication dialog
      window.location.href = '/login'; // Or your login page
    }
    return Promise.reject(error);
  }
);

export { agent };
```

### Step 4: Test Protected Endpoints
```typescript
// Example usage with protected endpoint
const userProfile = await agent.get('/api/user/profile');
console.log(userProfile.data); 
// { userId: "...", isAuthenticated: true, authenticationType: "Bearer" }
```

### Step 5: Handle Token Expiration (Optional but Recommended)
Add token refresh logic if you want long-lived sessions:

```typescript
// Decode JWT to check expiration
function isTokenExpired(token: string): boolean {
  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.exp * 1000 < Date.now();
  } catch {
    return true;
  }
}

// Check before making requests
if (isTokenExpired(localStorage.getItem('authToken') || '')) {
  localStorage.removeItem('authToken');
  // Redirect to re-authenticate
}
```

### Step 6: Update Login Flow

Your login flow should now:
1. Redirect to `/api/auth/login` (USOS login)
2. USOS redirects back to `/api/auth/callback` with OAuth parameters
3. Backend generates JWT and returns it
4. **Frontend extracts JWT and stores it** ← YOUR NEW STEP
5. Frontend redirects to dashboard/home and starts using JWT for API calls

### Step 7: Logout Flow
```typescript
function logout() {
  localStorage.removeItem('authToken');
  // Redirect to login page
  window.location.href = '/login';
}
```

---

## Configuration Checklist

### Backend `.env` file
```env
Jwt__Secret=your-secret-key-at-least-32-bytes-long
```

> ⚠️ **IMPORTANT for Production**:
> - Use a strong, random secret (min 32 bytes for HS256)
> - Store securely (never commit to git)
> - Rotate regularly
> - Use different secrets for dev/staging/production

### Frontend Usage
- Token should be sent in `Authorization: Bearer {token}` header
- Token expires in 24 hours (configurable via `ExpirationMinutes`)
- No CORS headers needed if backend is on same domain

---

## Testing the Implementation

### Manual Testing Steps

1. **Start Backend**:
   ```bash
   cd server
   dotnet restore
   dotnet run --project Presentation/Presentation.csproj
   ```

2. **Test Login Flow**:
   ```
   GET http://localhost:5000/api/auth/login
   → Redirects to USOS login
   → User authenticates with USOS
   → USOS redirects to http://localhost:5000/api/auth/callback?oauth_token=X&oauth_verifier=Y
   → Backend returns: { token: "...", userId: "...", hashedUsosId: "..." }
   ```

3. **Test Protected Endpoint with Token**:
   ```bash
   # With valid token
   curl -H "Authorization: Bearer {token}" http://localhost:5000/api/user/profile
   # Response: { userId: "...", isAuthenticated: true, authenticationType: "Bearer" }
   
   # Without token
   curl http://localhost:5000/api/user/profile
   # Response: 401 Unauthorized
   
   # With invalid token
   curl -H "Authorization: Bearer invalid" http://localhost:5000/api/user/profile
   # Response: 401 Unauthorized
   ```

---

## Important Notes

1. **Token Anonymity**: The JWT contains ONLY the internal database UUID, not the USOS ID. This ensures user privacy.

2. **Token Expiration**: Tokens expire after 24 hours. Design your UX to handle re-authentication gracefully.

3. **Security**: 
   - Always use HTTPS in production
   - Store tokens securely (HTTPOnly cookies preferred)
   - Don't expose tokens in URLs or logs

4. **CORS**: If frontend and backend are on different domains, ensure CORS is configured appropriately.

5. **Future Enhancements**:
   - Add refresh token support for longer sessions
   - Implement token blacklist/revocation
   - Add rate limiting to authentication endpoints

---

## Architecture Compliance

This implementation follows your existing architecture:
- ✅ **Clean Architecture**: JWT service in Infrastructure, interface in Application
- ✅ **Dependency Inversion**: Controllers depend on interfaces, not implementations
- ✅ **Domain Isolation**: No external JWT details leak into Domain layer
- ✅ **Error Handling**: Uses your existing `FluentResults` error handling pattern
- ✅ **Configuration Management**: Follows your settings pattern

