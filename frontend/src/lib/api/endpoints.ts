import { putJson, post, get, type FetchFn } from "./fetch";

type SetSpotifyCredentialsRequest = {
    clientId: string;
    clientSecret: string;
    redirectUri: string;
};

type SetSpotifyCredentialsResponse = {
    authorizationUri: string;
};

export async function setSpotifyCredentials(fetch: FetchFn, request: SetSpotifyCredentialsRequest) {
    return await putJson<SetSpotifyCredentialsResponse>('/spotify/credentials', request, fetch);
}



type ExchangeAuthorizationCodeRequest = {
    code: string;
    state: string;
};
export async function exchangeAuthorizationCode(fetch: FetchFn, request: ExchangeAuthorizationCodeRequest) {
    return await post('/spotify/authorize', request, fetch);
}


export async function isSpotifyCredentialsSet(fetch: FetchFn) {
    const response = await get('/spotify/credentials', fetch);

    if (response.ok) {
        return true;
    }

    if (response.isHttpError && response.error.status === 404) {
        return false;
    }

    return response;
}