import { getJson, type FetchFn } from "$lib/api/fetch";
import type { SpotifyArtist } from "$lib/api/types/spotify";

export default async function getTopTracks(fetch: FetchFn) {
    return await getJson<SpotifyArtist[]>(fetch, "/spotify/artists");
}