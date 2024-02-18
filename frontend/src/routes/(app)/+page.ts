import { getRFIDs } from '$lib/api/endpoints';
import { error, redirect } from '@sveltejs/kit';
import type { PageLoad } from './$types';

export const load = (async ({ fetch }) => {

    const response = await getRFIDs(fetch);
    if (!response.ok) {
        error(500, 'Failed to load RFIDs');
    }

    if (response.data.length === 0) {
        redirect(307, '/rfid/setup')
    }

    return {
        rfids: response.data,
    };
}) satisfies PageLoad;