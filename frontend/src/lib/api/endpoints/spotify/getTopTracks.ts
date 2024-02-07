import { getJson, type FetchFn } from "$lib/api/fetch";
import type { SpotifyTrack } from "$lib/api/types/spotify";

export default async function getTopTracks(fetch: FetchFn) {
    return await getJson<SpotifyTrack[]>(fetch, "/spotify/tracks");
}
