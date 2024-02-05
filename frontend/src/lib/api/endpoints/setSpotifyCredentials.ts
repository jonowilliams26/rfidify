import { put } from '$lib/api/fetch';

type SetSpotifyCredentialsRequest = {
    clientId: string;
    clientSecret: string;
    redirectUri: string;
}
type SetSpotifyCredentialsResponse = {
    authorizationUri: string;
}
export async function setSpotifyCredentials(request: SetSpotifyCredentialsRequest) {
    return await put<SetSpotifyCredentialsResponse>('/spotify/credentials', request);
}