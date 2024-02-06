import { get, type FetchFn, type Error } from "$lib/api/fetch";

type IsSpotifyCredentialsSetResponse = boolean | Error

export default async function isSpotifyCredentialsSet(fetch: FetchFn) : Promise<IsSpotifyCredentialsSetResponse> {
    const response = await get(fetch, "/spotify/credentials");
    if (response.ok) {
        return true;
    }

    if (response.isHttpError && response.error.status === 404) {
        return false
    }

    return response;
}