import { error } from '@sveltejs/kit';
import type { PageLoad } from './$types';
import type { FetchFn } from '$lib/api/fetch';
import { isSpotifyItemType } from '$lib/api/types';
import type { PagedResponse, SpotifyItem, SpotifyItemType } from '$lib/api/types';
import { getTopTracks } from '$lib/api/endpoints';

export const load = (async ({ fetch, url }) => {

    const type = url.searchParams.get('type') ?? 'Track';
    const search = url.searchParams.get('search');

    if (!isSpotifyItemType(type)) {
        error(400, "Invalid type");
    }

    return {
        // Need to add a no-op catch since we are streaming the results
        // See: https://kit.svelte.dev/docs/load#streaming-with-promises
        spotifyItems: getSpotifyItems(fetch, type, search).catch(() => {})
    };

}) satisfies PageLoad;

async function getSpotifyItems(fetch: FetchFn, type: SpotifyItemType, search: string | null) : Promise<PagedResponse<SpotifyItem>> {
    const response = await getTopTracks(fetch);
    if (!response.ok) {
        return Promise.reject(response);
    }
    return response.data;
}