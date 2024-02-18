import { error } from '@sveltejs/kit';
import type { PageLoad } from './$types';
import { isSpotifyItemType } from '$lib/api/types';
import { SpotifyItemTypes } from '$lib/api/types';
import { getSpotifyItems } from '$lib/api/endpoints';

export const load = (async ({ fetch, url }) => {

    const type = url.searchParams.get('type') ?? SpotifyItemTypes.track;
    const search = url.searchParams.get('search');

    if (!isSpotifyItemType(type)) {
        error(400, "Invalid type");
    }

    return {
        // Need to add a no-op catch since we are streaming the results
        // See: https://kit.svelte.dev/docs/load#streaming-with-promises
        spotifyItems: getSpotifyItems(fetch, type, search)
            .then((response) => {
                if (response.ok) {
                    return response.data;
                }
            })
            .catch(() => undefined)
    };

}) satisfies PageLoad;