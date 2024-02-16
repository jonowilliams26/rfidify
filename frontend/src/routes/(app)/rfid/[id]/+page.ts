import { error } from '@sveltejs/kit';
import type { PageLoad } from './$types';
import type { ApiResponseWithData, FetchFn } from '$lib/api/fetch';
import { isSpotifyItemType } from '$lib/api/types';
import type { PagedResponse, SpotifyItem, SpotifyItemType } from '$lib/api/types';
import { getPlaylists, getSavedAlbums, getTopArtists, getTopTracks } from '$lib/api/endpoints';

export const load = (async ({ fetch, url }) => {

    const type = url.searchParams.get('type') ?? 'Track';
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
    let response: ApiResponseWithData<PagedResponse<SpotifyItem>>;
    switch (type) {
        case 'Track':
            response = await getTopTracks(fetch);
            break;
        case 'Artist':
            response = await getTopArtists(fetch);
            break;
        case 'Album':
            response = await getSavedAlbums(fetch);
            break;
        case 'Playlist':
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