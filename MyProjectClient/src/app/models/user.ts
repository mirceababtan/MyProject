export interface User {
  username: string;
  email: string;
  password?: string;
}

export interface RegisterData {
  username: string;
  email: string;
  password: string;
}
