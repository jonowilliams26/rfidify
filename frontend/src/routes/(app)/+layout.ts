import isSpotifyCredentialsSet from '$lib/api/endpoints/spotify/isSpotifyCredentialsSet';
import { error, redirect } from '@sveltejs/kit';
import type { LayoutLoad } from './$types';

export const load = (async ({ fetch }) => {

    const result = await isSpotifyCredentialsSet(fetch);

    if (result === true) {
        return {};
    }

    if (result === false) {
        redirect(301, '/setup');
    }

    error(500, 'Failed to check Spotify credentials');
}) satisfies LayoutLoad;
