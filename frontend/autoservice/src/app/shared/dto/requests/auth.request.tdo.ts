export interface LoginRequestDto {
  username: string;
  password: string;
}
 
export interface CreateUserRequestDto {
  firstName: string;
  lastName: string;
  username: string;
  password: string;
  role: string; 
}