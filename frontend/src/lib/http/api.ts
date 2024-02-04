import { put, post } from "./fetch";

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

type AuthorizeSpotifyRequest = {
    code: string;
    state: string;
}
export async function authorizeSpotify(request: AuthorizeSpotifyRequest) {
    return await post('/spotify/authorize', request);
}