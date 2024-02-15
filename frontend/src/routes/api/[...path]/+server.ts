import type { RequestHandler } from './$types';
import { PRIVATE_API_URL } from "$env/static/private"

export const fallback: RequestHandler = async ({ request, fetch, url }) => {
    request = new Request(
        url.pathname.replace('/api', PRIVATE_API_URL),
        request,
    );
    return fetch(request);
};