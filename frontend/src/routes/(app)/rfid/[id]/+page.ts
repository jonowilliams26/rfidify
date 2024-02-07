import type { PageLoad } from './$types';
import { error } from '@sveltejs/kit';
import type { ApiResponseWithData } from '$lib/api/fetch';
import getTopTracks from '$lib/api/endpoints/spotify/getTopTracks';
import getTopArtists from '$lib/api/endpoints/spotify/getTopArtists';
import type { SpotifyItem } from '$lib/api/types/spotify';

export const load = (async ({ params, url, fetch }) => {

    const type = url.searchParams.get('type') ?? 'tracks';
    const search = url.searchParams.get('search')
    
    let response: ApiResponseWithData<SpotifyItem[]>;
    switch (type) {
        case 'tracks':
            response = await getTopTracks(fetch);
            break;
        case 'artists':
            response = await getTopArtists(fetch);
            break;
        default:
            error(400, 'Invalid type');
    }

    if (!response.ok) {
        error(500, 'Failed to load items');
    }

    return {
        items: response.data,
        loadingItems: false,
    }

}) satisfies PageLoad;