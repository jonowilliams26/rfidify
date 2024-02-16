import { putJson, post, get, getJson, $delete, type FetchFn } from "./fetch";
import type { PagedResponse, RFID, Track } from "./types";

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

export async function getRFIDs(fetch: FetchFn) {
    return await getJson<RFID[]>('/rfids', fetch);
}

export async function getRFID(fetch: FetchFn, id: string) {
    return await getJson<RFID>(`/rfids/${id}`, fetch);
}

export async function deleteRFID(fetch: FetchFn, id: string) {
    return await $delete(`/rfids/${id}`, fetch);
}

export async function getTopTracks(fetch: FetchFn) {
    return await getJson<PagedResponse<Track>>('/spotify/tracks', fetch);
}