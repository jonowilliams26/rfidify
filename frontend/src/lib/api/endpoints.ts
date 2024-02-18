import { putJson, post, get, getJson, $delete, type FetchFn, put, type ApiResponseWithData } from "./fetch";
import { SpotifyItemTypes, type Album, type Artist, type PagedResponse, type Playlist, type RFID, type SpotifyItem, type SpotifyItemType, type Track } from "./types";

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

export async function getTopTracks(fetch: FetchFn, offset?: number) {
    const uri = addOffset('/spotify/tracks', offset);
    return await getJson<PagedResponse<Track>>(uri, fetch);
}

export async function getTopArtists(fetch: FetchFn, offset?: number) {
    const uri = addOffset('/spotify/artists', offset);
    return await getJson<PagedResponse<Artist>>(uri, fetch);
}

export async function getPlaylists(fetch: FetchFn, offset?: number) {
    const uri = addOffset('/spotify/playlists', offset);
    return await getJson<PagedResponse<Playlist>>(uri, fetch);
}

export async function getSavedAlbums(fetch: FetchFn, offset?: number) {
    const uri = addOffset('/spotify/albums', offset);
    return await getJson<PagedResponse<Album>>(uri, fetch);
}

function addOffset(uri: string, offset?: number) {
    const searchParams = new URLSearchParams();
    if (offset) {
        searchParams.set('offset', offset.toString());
    }
    return `${uri}?${searchParams}`;
}


type CreateOrUpdateRFIDRequest = {
    rfid: string;
    spotifyUri: string;
}
export async function createOrUpdateRFID(fetch: FetchFn, request: CreateOrUpdateRFIDRequest) {
    return await put('/rfids', request, fetch);
}

export async function searchForSpotifyItems(fetch: FetchFn, search: string, type: SpotifyItemType, offset?: number) {
    const searchParams = new URLSearchParams();
    searchParams.set('search', search);
    searchParams.set('type', type);
    if (offset) {
        searchParams.set('offset', offset.toString());
    }
    const uri = `/spotify/search?${searchParams}`;
    return await getJson<PagedResponse<SpotifyItem>>(uri, fetch);
}

export async function getSpotifyItems(fetch: FetchFn, type: SpotifyItemType, search?: string | null, offset?: number): Promise<ApiResponseWithData<PagedResponse<SpotifyItem>>> {

    if (search) {
        return await searchForSpotifyItems(fetch, search, type, offset);
    }

    switch (type) {
        case SpotifyItemTypes.track:
            return await getTopTracks(fetch, offset);
        case SpotifyItemTypes.artist:
            return await getTopArtists(fetch, offset);
        case SpotifyItemTypes.album:
            return await getSavedAlbums(fetch, offset);
        case SpotifyItemTypes.playlist:
            return await getPlaylists(fetch, offset);
        default:
            const exhaustiveCheck: never = type;
            return Promise.reject(exhaustiveCheck);
    }
}