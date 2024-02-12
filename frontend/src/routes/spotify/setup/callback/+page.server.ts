import { error, redirect } from '@sveltejs/kit';
import type { PageServerLoad } from './$types';
import { exchangeAuthorizationCode } from '$lib/api/endpoints';

export const load = (async ({ fetch, url }) => {

    const code = url.searchParams.get('code');
    const state = url.searchParams.get('state');
    const errorCode = url.searchParams.get('error');

    if (errorCode) {
        error(500, `Spotify returned an error: ${errorCode}`);
    }

    if (!code || !state) {
        redirect(301, '/spotify/setup');
    }

    const response = await exchangeAuthorizationCode(fetch, { code, state });
    if (!response.ok) {
        error(500, 'Failed to exchange authorization code');
    }

    redirect(301, '/');
}) satisfies PageServerLoad;