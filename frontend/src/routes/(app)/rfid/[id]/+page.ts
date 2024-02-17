import { error } from '@sveltejs/kit';
import type { PageLoad } from './$types';
import type { ApiResponseWithData, FetchFn } from '$lib/api/fetch';
import { isSpotifyItemType } from '$lib/api/types';
import { SpotifyItemTypes, type PagedResponse, type SpotifyItem, type SpotifyItemType } from '$lib/api/types';
import { getPlaylists, getSavedAlbums, getTopArtists, getTopTracks, searchForSpotifyItems } from '$lib/api/endpoints';

export const load = (async ({ fetch, url }) => {

    const type = url.searchParams.get('type') ?? SpotifyItemTypes.track;
    const search = url.searchParams.get('search');

    if (!isSpotifyItemType(type)) {
        error(400, "Invalid type");
    }

    return {
        // Need to add a no-op catch since we are streaming the results
        // See: https://kit.svelte.dev/docs/load#streaming-with-promises
        spotifyItems: getSpotifyItems(fetch, type, search).catch(() => { })
    };

}) satisfies PageLoad;

async function getSpotifyItems(fetch: FetchFn, type: SpotifyItemType, search: string | null): Promise<PagedResponse<SpotifyItem>> {
    
    if(search){
        const response = await searchForSpotifyItems(fetch, search, type);
        if (!response.ok) {
            return Promise.reject(response);
        }
        return response.data;
    }

    
    let response: ApiResponseWithData<PagedResponse<SpotifyItem>>;
    switch (type) {
        case SpotifyItemTypes.track:
            response = await getTopTracks(fetch);
            break;
        case SpotifyItemTypes.artist:
            response = await getTopArtists(fetch);
            break;
        case SpotifyItemTypes.album:
            response = await getSavedAlbums(fetch);
            break;
        case SpotifyItemTypes.playlist:
            response = await getPlaylists(fetch);
            break;
        default:
            const exhaustiveCheck: never = type;
            return Promise.reject(exhaustiveCheck);
    }

    if (!response.ok) {
        return Promise.reject(response);
    }
    
    return response.data;
}