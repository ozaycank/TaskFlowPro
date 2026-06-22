import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';

const publicRoutes = ['/login', '/register', '/forgot-password'];

// FIXED: Changed function name from 'middleware' to 'proxy' to match Next.js 16+ API.
export function proxy(request: NextRequest) {
    const { pathname } = request.nextUrl;

    // Skip API routes and static assets
    if (pathname.startsWith('/api') || pathname.startsWith('/_next') || pathname.includes('.')) {
        return NextResponse.next();
    }

    const isPublicRoute = publicRoutes.some(route => pathname.startsWith(route));
    const hasRefreshTokenCookie = request.cookies.has('refreshToken'); // Adjust cookie name based on backend

    // If user tries to access a protected route without a token cookie
    if (!isPublicRoute && !hasRefreshTokenCookie) {
        const url = new URL('/login', request.url);
        url.searchParams.set('callbackUrl', pathname);
        return NextResponse.redirect(url);
    }

    // If user is logged in and tries to access login page
    if (isPublicRoute && hasRefreshTokenCookie) {
        return NextResponse.redirect(new URL('/', request.url));
    }

    return NextResponse.next();
}

export const config = {
    matcher: ['/((?!api|_next/static|_next/image|favicon.ico).*)'],
};