import { post } from '$lib/api/fetch';

type AuthorizeSpotifyRequest = {
    code: string;
    state: string;
}
export async function authorizeSpotify(request: AuthorizeSpotifyRequest) {
    return await post('/spotify/authorize', request);
}