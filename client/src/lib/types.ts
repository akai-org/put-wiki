export interface IOauthToken {
  token: string;
  userId: string;
  hashedUsosId: string;
}

export interface IAuthSession {
  // Later, we can store here user's data or use React Query
  userId: string | null;
}
