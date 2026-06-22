export interface UserDto {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    avatarUrl?: string;
    isActive: boolean;
}

export interface LoginResponseDto {
    accessToken: string;
    // Refresh token is handled via HttpOnly Cookie, so it's not here.
    user: UserDto;
}

export interface ProblemDetails {
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
    errors?: Record<string, string[]>;
}