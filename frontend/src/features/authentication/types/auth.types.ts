export interface UserDto {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    avatarUrl?: string;
    isActive: boolean;
}

export interface LoginResponseDto {
    userId: string;
    email: string;
    firstName: string;
    lastName: string;
    accessToken: string;
    refreshToken: string;
}

export interface ProblemDetails {
    type?: string;
    title?: string;
    status?: number;
    detail?: string;
    instance?: string;
    errors?: Record<string, string[]>;
}