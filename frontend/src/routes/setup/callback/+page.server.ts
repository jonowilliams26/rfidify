import type { PageServerLoad } from './$types';
import { authorizeSpotify } from '$lib/api/endpoints/authorizeSpotify';
import { error, redirect } from '@sveltejs/kit';

export const load = (async ({ url }) => {

    const code = url.searchParams.get('code');
    const state = url.searchParams.get('state');
    const errorState = url.searchParams.get('error');

    if (errorState) {
        error(401, errorState);
    }

    if (!code || !state) {
        error(401, 'Invalid code or state');
    }

    const response = await authorizeSpotify({ code, state });
    if (!response.ok) {
        error(500, 'Failed to authorize Spotify');
    }

    redirect(301, '/');   
}) satisfies PageServerLoad;