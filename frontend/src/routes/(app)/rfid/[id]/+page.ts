import { getRFID } from '$lib/api/endpoints';
import { error } from '@sveltejs/kit';
import type { PageLoad } from './$types';

export const load = (async ({ fetch, params }) => {

    const response = await getRFID(fetch, params.id);

    if (!response.ok && response.isHttpError && response.error.status === 404) {
        return {
            rfid: {
                id: params.id,
                spotifyItem: undefined
            }
        };
    }

    if (!response.ok) {
        error(500, "Failed to load RFID");
    }

    return {
        rfid: response.data
    };

}) satisfies PageLoad;