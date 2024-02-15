import { putJson, post, get, getJson, type FetchFn } from "./fetch";

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


type RFID = {
    id: string;
    spotifyItem: Track | Artist | Album | Playlist;
}

type Track = {
    type: "Track";
    id: string;
    name: string;
    uri: string;
}

type Artist = {
    type: "Artist";
    id: string;
    name: string;
    uri: string;
}

type Album = {
    type: "Album";
    id: string;
    name: string;
    uri: string;
}

type Playlist = {
    type: "Playlist";
    id: string;
    name: string;
    uri: string;
    description: string;
}

type Image = {
    url: string;
    width?: number;
    height?: number;
}

export async function getRFIDs(fetch: FetchFn) {
    return await getJson<RFID[]>('/rfids', fetch);
}