export interface LoginResponseDto {
  token: string;
  role: string;
  fullName: string;
}
 
export interface CreateUserResponseDto {
  id: string;
  firstName: string;
  lastName: string;
  username: string;
  role: string;
  token: string;
}